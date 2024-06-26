
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

namespace Fluent.CodeGen
{
    public abstract class CodeGen
    {
        protected IndentedTextWriter indentedTextWriter;
        protected StringWriter stringWriter;

        public CodeGen()
        {
            Flush();
        }

        protected void Flush()
        {
            stringWriter?.Dispose();
            indentedTextWriter?.Dispose();

            stringWriter = new StringWriter();
            indentedTextWriter = new IndentedTextWriter(stringWriter);
            indentedTextWriter.NewLine = "\n";
        }

        protected void WriteMultipleLines(string body)
        {
            var lines = body.Split("\n").ToList();
            var last = lines.Last();
            lines.ForEach(line =>
            {
                if(line.Equals(last) && string.IsNullOrEmpty(line))
                {
                    return;
                }
                if(string.IsNullOrWhiteSpace(line))
                {
                    var indentation = indentedTextWriter.Indent;
                    indentedTextWriter.Indent = 0;
                    indentedTextWriter.WriteLine();
                    indentedTextWriter.Indent = indentation;
                }
                else
                {
                    indentedTextWriter.WriteLine(line);
                }
            });
        }

        protected void WriteNewLineNoIndentation()
        {
            var indent = indentedTextWriter.Indent;
            indentedTextWriter.Indent = 0;
            indentedTextWriter.WriteLine();
            indentedTextWriter.Indent = indent;
        }

        public abstract string GenerateCode();

        public string GenerateCode(int indentation)
        {
            string generatedCode = string.Empty;
            int previousIndentation = indentedTextWriter.Indent;
            indentedTextWriter.Indent = indentation;
            generatedCode = GenerateCode();
            indentedTextWriter.Indent = previousIndentation;
            return generatedCode;
        }
    }
}