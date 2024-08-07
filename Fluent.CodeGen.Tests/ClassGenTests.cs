using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen.Tests
{
    public class ClassGenTests
    {

        [Fact]
        public void TestClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .GenerateCode();

            var expectedCode = """"
                using System;
                
                class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("Program", classGen.ClassName);
            Assert.Single(classGen.Usings);
            Assert.Empty(classGen.Fields);
            Assert.Empty(classGen.Properties);
            Assert.False(classGen.IsStatic);
        }

        [Fact]
        public void TestDuplicateUsing()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Using("System")
                .GenerateCode();

            var expectedCode = """"
                using System;
                
                class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Single(classGen.Usings);
        }

        [Fact]
        public void TestClassWithoutNamespaceAndMultipleUsings()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System", "System.Text")
                .GenerateCode();

            var expectedCode = """"
                using System;
                using System.Text;

                class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Null(classGen.GetNamespace());
            Assert.Equal(2, classGen.Usings.Count);
        }


        [Fact]
        public void TestPublicClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Public()
                .GenerateCode();

            var expectedCode = """"
                using System;

                public class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("public", classGen.AccessModifier);
        }



        [Fact]
        public void TestStaticClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Public()
                .Static()
                .GenerateCode();

            var expectedCode = """"
                using System;

                public static class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.True(classGen.IsStatic);
        }


        [Fact]
        public void TestNormalClass()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("My.Test", classGen.GetNamespace());
        }



        [Fact]
        public void TestExtends()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Extends("MyClass")
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program : MyClass
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("MyClass", classGen.Inherits);
        }



        [Fact]
        public void TestExtendsAndImplements()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Extends("MyClass")
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program : MyClass, IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("MyClass", classGen.Inherits);
            Assert.Contains("IInterface", classGen.Implementations);
            Assert.Contains("IInterface2", classGen.Implementations);
            Assert.Equal(2, classGen.Implementations.Count);
        }


        [Fact]
        public void TestImplements()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Null(classGen.Inherits);
            Assert.Contains("IInterface", classGen.Implementations);
            Assert.Contains("IInterface2", classGen.Implementations);
            Assert.Equal(2, classGen.Implementations.Count);
        }



        [Fact]
        public void TestStatic()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Static()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public static class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        public void TestDefaultStatic()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Static()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();


            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    static class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        public void TestInternalClass()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Internal()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    internal class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("internal", classGen.AccessModifier);
        }

        [Fact]
        public void TestClassWithMethod()
        {
            var methodGen = new MethodGen(name: "TestMethod")
                .Public()
                .WithReturnType("int")
                .WithBody("return 0;");

            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithMethod(methodGen)
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                        public int TestMethod()
                        {
                            return 0;
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
            Assert.Single(classGen.Methods);
            Assert.Contains("TestMethod", classGen.Methods.Select(x => x.Name));
            Assert.Contains("int", classGen.Methods.Select(x => x.ReturnType));
            Assert.Contains("public", classGen.Methods.Select(x => x.AccessModifier));
        }

        [Fact]
        public void TestClassWithFields()
        {
            var fieldTest = new FieldGen("string", "test")
                .Public()
                .Static();

            var fieldAmount = new FieldGen("int", "amount")
                .Assign("10");

            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithField(fieldTest)
                .WithField(fieldAmount)
                .Constructor(ctor => ctor.Public())
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                        public static string test;
                        int amount = 10;

                        public Program()
                        {
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        public void TestClassWithProperty()
        {
            var propertyTest = new PropertyGen("string", "Test")
                .Public()
                .Static();

            var propertyAmount = new PropertyGen("int", "Amount")
                .Set(AccessModifiers.Private, "")
                .Assign("10");

            var getBody = """
                string final = DateTime.Now.ToShortDateString() + " - " + Test;
                return final;
                """;

            var setBody = """
                string final = value.ToLower();
                this.Test = final;
                """;

            var propertyLog = new PropertyGen(type: "string", name: "Log")
                .Public()
                .Get(getBody)
                .Set(setBody);


            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithProperty(propertyTest)
                .WithProperty(propertyAmount)
                .WithProperty(propertyLog)
                .Constructor(ctor => ctor.Public())
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                        public static string Test { get; set; }
                        int Amount { get; private set; } = 10;
                        public string Log
                        {
                            get
                            {
                                string final = DateTime.Now.ToShortDateString() + " - " + Test;
                                return final;
                            }
                            set
                            {
                                string final = value.ToLower();
                                this.Test = final;
                            }
                        }

                        public Program()
                        {
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        void ReplaceFieldsIfSameName()
        {
            var fieldAmount = new FieldGen("int", "amount")
                .Assign("10");

            var fieldAmount2 = new FieldGen("int", "amount");

            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithField(fieldAmount)
                .WithField(fieldAmount2)
                .Constructor(ctor => ctor.Public())
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                        int amount;

                        public Program()
                        {
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        void ReplacePropertyIfSameName()
        {
            var propertyAmount = new PropertyGen("int", "Amount")
                .Assign("10");

            var propertyAmount2 = new PropertyGen("int", "Amount");

            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithProperty(propertyAmount)
                .WithProperty(propertyAmount2)
                .Constructor(ctor => ctor.Public())
                .GenerateCode();

            var expectedCode = """"
                using System;

                namespace My.Test
                {
                    public class Program
                    {
                        int Amount { get; set; }

                        public Program()
                        {
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        public void TestClassWithAttribute()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
            .Using("System")
            .WithAttributes("[MyCustomAttribute]")
            .GenerateCode();

            var expectedCode = """"
            using System;

            [MyCustomAttribute]
            class Program
            {
            }
            """";
            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        public void TestAddMultipleAttributes()
        {
            var classGen = new ClassGen("TestClass");
            var generatedCode = classGen
                .Using("System")
                .WithAttributes("[MyCustomAttribute]", "[AnotherCustomAttribute2]")
                .GenerateCode();

            var expectedCode = """"
            using System;
            
            [MyCustomAttribute]
            [AnotherCustomAttribute2]
            class TestClass
            {
            }
            """";
            Assert.Equal(expectedCode, generatedCode);
        }
    }
}