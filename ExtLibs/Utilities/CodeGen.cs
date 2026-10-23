using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp;
using MissionPlanner.Utilities;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Digests;

namespace MissionPlanner
{
    public static class CodeGenRoslyn
    {
        public static string lasterror = "";

        public static string CachePath = Settings.GetDataDirectory() + "plugins" + Path.DirectorySeparatorChar;

        public static Assembly BuildCode(string filepath)
        {
            try
            {
                Directory.CreateDirectory(CachePath);
            }
            catch
            {
            }

            var md5hash = "";
            lasterror = "";
            var filecontents = File.ReadAllText(filepath, Encoding.UTF8);
            {
                var bytes = filecontents.Select(a => (byte) a).ToArray();
                var md5 = new MD5Digest();
                md5.BlockUpdate(bytes, 0, bytes.Length);
                var result = new byte[md5.GetDigestSize()];
                md5.DoFinal(result, 0);
                md5hash = BitConverter.ToString(result).Replace("-", "").ToLower();

                if (File.Exists(Path.Combine(CachePath, md5hash + ".dll")) && File.Exists(Path.Combine(CachePath, md5hash + ".pdb")))
                {
                    // load the cached version
                    return Assembly.Load(File.ReadAllBytes(Path.Combine(CachePath, md5hash + ".dll")),
                        File.ReadAllBytes(Path.Combine(CachePath, md5hash + ".pdb")));
                }
            }
            var syntaxTree =
                CSharpSyntaxTree.ParseText(filecontents, path: filepath,
                    encoding: Encoding.UTF8);
            var assemblyName = Path.GetFileNameWithoutExtension(filepath); //Guid.NewGuid().ToString();

            var refs = AppDomain.CurrentDomain.GetAssemblies();
            var refFiles = refs.Where(a =>
                    !a.IsDynamic && !a.FullName.Contains("MissionPlanner.Drawing"))
                .Select(a => a.Location);
            // also reference all assemblies in the app base directory, so plugins can
            // use types from assemblies not yet loaded into the AppDomain
            // (e.g. MissionPlanner.Strings referenced by localized plugin sources)
            try
            {
                refFiles = refFiles.Concat(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                    .Where(f => f != "" && File.Exists(f) &&
                                !Path.GetFileName(f).Contains("MissionPlanner.Drawing"))
                    .Distinct();
            }
            catch
            {
            }
            // netstandard facade is required so that plugins referencing netstandard-targeted
            // assemblies (e.g. DroneCAN) can resolve forwarded types (IEnumerable/Object etc.).
            // Prefer the pure reference-assembly facade: the .NET Framework reference-assembly
            // facade only type-forwards into mscorlib/System. Assembly.Load("netstandard") on
            // .NET Framework 4.7.1+ resolves the GAC contract assembly instead, which also
            // DEFINES the netstandard2.0 surface - passing that alongside mscorlib breaks
            // Roslyn's predefined-type resolution (CS0433/CS0518). Use the facade first, and
            // only fall back to Assembly.Load when no facade is installed on the machine.
            var netstandardFacades = FindNetStandardFacade().ToArray();
            if (netstandardFacades.Length > 0)
            {
                refFiles = refFiles.Concat(netstandardFacades).Distinct();
            }
            else
            {
                try
                {
                    var nsasm = Assembly.Load("netstandard");
                    if (nsasm != null && !string.IsNullOrEmpty(nsasm.Location) && File.Exists(nsasm.Location))
                        refFiles = refFiles.Concat(new[] {nsasm.Location}).Distinct();
                }
                catch
                {
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                refFiles = refs.Where(a => !a.IsDynamic).Select(a => a.Location);
            var refmeta = refFiles.Select(a =>
            {
                try
                {
                    if (a == "" || !File.Exists(a))
                        return null;
                    // skip non-managed (native) PEs - they have no CLR metadata and would
                    // otherwise surface as CS0009 at emit time
                    AssemblyName.GetAssemblyName(a);
                    return AssemblyMetadata.CreateFromFile(a).GetReference();
                }
                catch
                {
                    return null;
                }
            }).Where(a => a != null).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                new[] {syntaxTree}, refmeta,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var dllStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(dllStream, pdbStream);
                if (!emitResult.Success)
                {
                    // emitResult.Diagnostics
                    emitResult.Diagnostics.ForEach(a => Console.WriteLine("CodeGenRoslyn " + Path.GetFileName(filepath) + ": {0}", a.ToString()));
                    lasterror = emitResult.Diagnostics.Aggregate("", (a, b) => a + b.ToString()+"\n");
                }
                else
                {
                    try
                    {
                        File.WriteAllBytes(Path.Combine(CachePath, md5hash + ".dll"), dllStream.ToArray());
                        File.WriteAllBytes(Path.Combine(CachePath, md5hash + ".pdb"), pdbStream.ToArray());
                    }
                    catch
                    {
                    }

                    return Assembly.Load(dllStream.ToArray(), pdbStream.ToArray());
                }

                return null;
            }
        }

        private static IEnumerable<string> FindNetStandardFacade()
        {
            var candidates = new List<string>();
            try
            {
                // Only the reference-assembly facades are pure type-forwarders (no type
                // definitions). The GAC contract assembly and the NuGet NETStandard.Library
                // ref assembly both define the netstandard2.0 surface and conflict with
                // mscorlib when passed together, so they are intentionally not candidates.
                foreach (var ver in new[] {"v4.8", "v4.7.2", "v4.7.1", "v4.7", "v4.6.2", "v4.6.1"})
                {
                    var facade = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                        "Reference Assemblies", "Microsoft", "Framework", ".NETFramework", ver, "Facades",
                        "netstandard.dll");
                    if (File.Exists(facade))
                        candidates.Add(facade);
                }

                // 64-bit VS installs on 64-bit systems put Reference Assemblies under ProgramFiles
                var progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                if (!string.Equals(progFiles, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)))
                {
                    foreach (var ver in new[] {"v4.8", "v4.7.2", "v4.7.1", "v4.7", "v4.6.2", "v4.6.1"})
                    {
                        var facade = Path.Combine(progFiles,
                            "Reference Assemblies", "Microsoft", "Framework", ".NETFramework", ver, "Facades",
                            "netstandard.dll");
                        if (File.Exists(facade))
                            candidates.Add(facade);
                    }
                }
            }
            catch
            {
            }

            return candidates.Distinct();
        }
    }

    public static class CodeGen
    {
        public static string lasterror = "";
        public static object runCode(string code)
        {
            lasterror = "";
            object answer = null;

            GetMathMemberNames();

            // change evaluation string to pick up Math class members
            string expression = RefineEvaluationString(code);

            // build the class using codedom
            BuildClass(expression);

            // compile the class into an in-memory assembly.
            // if it doesn't compile, show errors in the window
            CompilerResults results = CompileAssembly();

            Console.WriteLine("...........................\r\n");
            Console.WriteLine(_source.ToString());

            // if the code compiled okay,
            // run the code using the new assembly (which is inside the results)
            if (results != null && results.CompiledAssembly != null)
            {
                // run the evaluation function
                answer = RunCode(results);
            }
            else
            {
            }

            return answer;
        }

        public static CodeDomProvider CreateCompiler()
        {
            //Create an instance of the C# compiler   
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            //ICodeCompiler compiler = codeProvider.CreateCompiler();
            return codeProvider;
        }

        /// <summary>
        /// Creawte parameters for compiling
        /// </summary>
        /// <returns></returns>
        public static CompilerParameters CreateCompilerParameters()
        {
            var refs = AppDomain.CurrentDomain.GetAssemblies();
            var refFiles = refs.Where(a => !a.IsDynamic && !a.FullName.Contains("mscorlib") && !a.FullName.Contains("MissionPlanner.Drawing"))
                .Select(a => a.Location);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                refFiles = refs.Where(a => !a.IsDynamic && !a.FullName.Contains("mscorlib")).Select(a => a.Location);

            //add compiler parameters and assembly references
            CompilerParameters compilerParams = new CompilerParameters(refFiles.ToArray());
            compilerParams.CompilerOptions = "/target:library /langversion:5";
            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = true;
            compilerParams.IncludeDebugInformation = true;

            //add any aditional references needed
            //            foreach (string refAssembly in code.References)
            //              compilerParams.ReferencedAssemblies.Add(refAssembly);

            return compilerParams;
        }

        /// <summary>
        /// Compiles the code from the code string
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="parms"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static CompilerResults CompileCode(CodeDomProvider compiler, CompilerParameters parms, string source)
        {
            //actually compile the code
            CompilerResults results = compiler.CompileAssemblyFromSource(
                parms, source);

            //Do we have any compiler errors?
            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                    Console.WriteLine("CodeGen: Compile Error: Line: " + error.Line + ":" + error.Column + " " + error.ErrorText);

                lasterror = results.Errors.Flatten<CompilerError>().Aggregate("",
                    (a, error) => { return a + error.Line + ":" + error.Column + " " + error.ErrorText + "\n"; });
                return null;
            }

            return results;
        }

        public static CompilerResults CompileCodeFile(CodeDomProvider compiler, CompilerParameters parms,
            string filename)
        {
            //actually compile the code
            CompilerResults results = compiler.CompileAssemblyFromFile(parms, new string[]
            {
                filename
            });

            //Do we have any compiler errors?
            if (results.Errors.Count > 0)
            {
                bool iserror = false;
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine("CodeGen Compile " + Path.GetFileName(filename) + ": " + (error.IsWarning ? "Warning" : "Error") + ": Line: " + error.Line +
                                      ":" + error.Column + " " +
                                      error.ErrorText);

                    lasterror = results.Errors.Flatten<CompilerError>().Aggregate("",
                        (a, error2) => { return a + (error2.IsWarning ? "Warning" : "Error") + ": Line: " + error2.Line +
                                               ":" + error2.Column + " " +
                                               error2.ErrorText + "\n"; });
                    if (!error.IsWarning)
                        iserror = true;
                }

                if(iserror)
                    return null;
            }

            return results;
        }

        /// <summary>
        /// Need to change eval string to use .NET Math library
        /// </summary>
        /// <param name="eval">evaluation expression</param>
        /// <returns></returns>
        public static string RefineEvaluationString(string eval)
        {
            // look for regular expressions with only letters
            Regex regularExpression = new Regex("[a-zA-Z_]+");

            // track all functions and constants in the evaluation expression we already replaced
            ArrayList replacelist = new ArrayList();

            // find all alpha words inside the evaluation function that are possible functions
            MatchCollection matches = regularExpression.Matches(eval);
            foreach (Match m in matches)
            {
                // if the word is found in the math member map, add a Math prefix to it
                bool isContainedInMathLibrary = _mathMembersMap[m.Value.ToUpper()] != null;
                if (replacelist.Contains(m.Value) == false && isContainedInMathLibrary)
                {
                    eval = eval.Replace(m.Value, "Math." + _mathMembersMap[m.Value.ToUpper()]);
                }

                // we matched it already, so don't allow us to replace it again
                replacelist.Add(m.Value);
            }

            // return the modified evaluation string
            return eval;
        }

        /// <summary>
        /// Compiles the c# into an assembly if there are no syntax errors
        /// </summary>
        /// <returns></returns>
        public static CompilerResults CompileAssembly()
        {
            // create a compiler
            CodeDomProvider compiler = CreateCompiler();
            // get all the compiler parameters
            CompilerParameters parms = CreateCompilerParameters();
            // compile the code into an assembly
            CompilerResults results = CompileCode(compiler, parms, _source.ToString());
            return results;
        }

        static ArrayList _mathMembers = new ArrayList();
        static Hashtable _mathMembersMap = new Hashtable();

        public static void GetMathMemberNames()
        {
            // get a reflected assembly of the System assembly
            Assembly systemAssembly = Assembly.GetAssembly(typeof (System.Math));
            try
            {
                //cant call the entry method if the assembly is null
                if (systemAssembly != null)
                {
                    //Use reflection to get a reference to the Math class

                    Module[] modules = systemAssembly.GetModules(false);
                    Type[] types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the Math class
                    foreach (Type type in types)
                    {
                        if (type.Name == "Math")
                        {
                            // get all of the members of the math class and map them to the same member
                            // name in uppercase
                            MemberInfo[] mis = type.GetMembers();
                            foreach (MemberInfo mi in mis)
                            {
                                _mathMembers.Add(mi.Name);
                                _mathMembersMap[mi.Name.ToUpper()] = mi.Name;
                            }
                        }
                        //if the entry point method does return in Int32, then capture it and return it
                    }


                    //if it got here, then there was no entry point method defined.  Tell user about it
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:  An exception occurred while executing the script {0}", ex);
            }
        }

        /// <summary>
        /// Runs the Calculate method in our on-the-fly assembly
        /// </summary>
        /// <param name="results"></param>
        public static string RunCode(CompilerResults results)
        {
            Assembly executingAssembly = results.CompiledAssembly;
            try
            {
                //cant call the entry method if the assembly is null
                if (executingAssembly != null)
                {
                    object assemblyInstance = executingAssembly.CreateInstance("ExpressionEvaluator.Calculator");
                    //Use reflection to call the static Main function

                    Module[] modules = executingAssembly.GetModules(false);
                    Type[] types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the entry point method
                    foreach (Type type in types)
                    {
                        MethodInfo[] mis = type.GetMethods();
                        foreach (MethodInfo mi in mis)
                        {
                            if (mi.Name == "Calculate")
                            {
                                object result = mi.Invoke(assemblyInstance, null);
                                return result.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:  An exception occurred while executing the script {0}", ex);
            }
            return "";
        }


        public static CodeMemberField FieldVariable(string fieldName, string typeName, MemberAttributes accessLevel)
        {
            CodeMemberField field = new CodeMemberField(typeName, fieldName);
            field.Attributes = accessLevel;
            return field;
        }

        public static CodeMemberField FieldVariable(string fieldName, Type type, MemberAttributes accessLevel)
        {
            CodeMemberField field = new CodeMemberField(type, fieldName);
            field.Attributes = accessLevel;
            return field;
        }

        /// <summary>
        /// Very simplistic getter/setter properties
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="internalName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CodeMemberProperty MakeProperty(string propertyName, string internalName, Type type)
        {
            CodeMemberProperty myProperty = new CodeMemberProperty();
            myProperty.Name = propertyName;
            myProperty.Comments.Add(
                new CodeCommentStatement(String.Format("The {0} property is the returned result", propertyName)));
            myProperty.Attributes = MemberAttributes.Public;
            myProperty.Type = new CodeTypeReference(type);
            myProperty.HasGet = true;
            myProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName)));

            myProperty.HasSet = true;
            myProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName),
                    new CodePropertySetValueReferenceExpression()));

            return myProperty;
        }


        static StringBuilder _source = new StringBuilder();

        /// <summary>
        /// Main driving routine for building a class
        /// </summary>
        public static void BuildClass(string expression)
        {
            // need a string to put the code into
            _source = new StringBuilder();
            StringWriter sw = new StringWriter(_source);

            //Declare your provider and generator
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeGenerator generator = codeProvider.CreateGenerator(sw);
            CodeGeneratorOptions codeOpts = new CodeGeneratorOptions();

            CodeNamespace myNamespace = new CodeNamespace("ExpressionEvaluator");
            myNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //myNamespace.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));
            //myNamespace.Imports.Add(new CodeNamespaceImport("MissionPlanner.Utilities"));
            //myNamespace.Imports.Add(new CodeNamespaceImport("MissionPlanner"));

            //Build the class declaration and member variables			
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration();
            classDeclaration.IsClass = true;
            classDeclaration.Name = "Calculator";
            classDeclaration.Attributes = MemberAttributes.Public;
            classDeclaration.Members.Add(FieldVariable("answer", typeof (object), MemberAttributes.Private));

            //default constructor
            CodeConstructor defaultConstructor = new CodeConstructor();
            defaultConstructor.Attributes = MemberAttributes.Public;
            defaultConstructor.Comments.Add(new CodeCommentStatement("Default Constructor for class", true));
            defaultConstructor.Statements.Add(new CodeSnippetStatement("//TODO: implement default constructor"));

            classDeclaration.Members.Add(defaultConstructor);


            //property
            classDeclaration.Members.Add(MakeProperty("Answer", "answer", typeof (object)));

            //Our Calculate Method
            /*
            CodeMemberMethod myMethod = new CodeMemberMethod();
            myMethod.Name = "Calculate";
            myMethod.ReturnType = new CodeTypeReference(typeof(object));
            myMethod.Comments.Add(new CodeCommentStatement("Calculate an expression", true));
            myMethod.Attributes = MemberAttributes.Public;
            myMethod.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression("Object obj"), new CodeSnippetExpression(expression)));
            //myMethod.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression("Answer"), new CodeSnippetExpression("obj.ToString()")));
            myMethod.Statements.Add(
                           new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Answer")));
            classDeclaration.Members.Add(myMethod);
             */

            classDeclaration.Members.Add(FieldVariable("customforusenumber", typeof (double), MemberAttributes.Public));
            classDeclaration.Members.Add(FieldVariable("customforuseobject", typeof (object), MemberAttributes.Public));

            CodeSnippetTypeMember myMethod = new CodeSnippetTypeMember();

            myMethod.Text = expression;

            classDeclaration.Members.Add(myMethod);

            //write code
            myNamespace.Types.Add(classDeclaration);
            generator.GenerateCodeFromNamespace(myNamespace, sw, codeOpts);
            sw.Flush();
            sw.Close();

            Console.Write(sw.ToString());
        }
    }
}