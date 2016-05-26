using System;
using System.Collections.Generic;
using System.IO;

namespace TypescriptGenerator.Utils
{
    class CodeWriter
    {
        private readonly List<string> _lines = new List<string>();
        string _indent = "";

        public void AddLine(string line)
        {
            _lines.Add(_indent + line);
        }

        public void WriteLine()
        {
            _lines.Add("");
        }

        public void SaveToFile(string fileName)
        {
            File.WriteAllLines(fileName, _lines);
        }

        public IDisposable CreateBlock(string terminator = null)
        {
            return new Block(this, terminator);
        }

        class Block: IDisposable
        {
            private readonly CodeWriter _codeWriter;
            private readonly string _terminator;

            public Block(CodeWriter codeWriter, string terminator)
            {
                _codeWriter = codeWriter;
                _terminator = terminator;
                if (_codeWriter._lines.Count == 0)
                    _codeWriter.AddLine("{");
                else
                {
                    var lastLine = _codeWriter._lines[_codeWriter._lines.Count - 1];
                    lastLine += " {";
                    _codeWriter._lines.RemoveAt(_codeWriter._lines.Count - 1);
                    _codeWriter._lines.Add(lastLine);
                }
                _codeWriter._indent += "    ";
            }

            public void Dispose()
            {
                _codeWriter._indent = _codeWriter._indent.Substring(0, _codeWriter._indent.Length - 4);
                _codeWriter.AddLine("}" + _terminator);
            }
        }
    }
}
