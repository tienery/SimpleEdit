using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace SimpleEdit
{
    public class Editor : FastColoredTextBox
    {

        protected static readonly Platform platformType = PlatformType.GetOperationSystemPlatform();

        public static RegexOptions RegexCompiledOption
        {
            get
            {
                if (platformType == Platform.X86)
                    return RegexOptions.Compiled;
                else
                    return RegexOptions.None;
            }
        }

        private Style comments = new TextStyle(Brushes.Green, Brushes.Transparent, FontStyle.Regular);
        private Style classes = new TextStyle(Brushes.DarkCyan, Brushes.Transparent, FontStyle.Regular);
        private Style keywords = new TextStyle(Brushes.Blue, Brushes.Transparent, FontStyle.Regular);
        private Style meta = new TextStyle(Brushes.Gray, Brushes.Transparent, FontStyle.Regular);
        private Style strings = new TextStyle(Brushes.DarkOrange, Brushes.Transparent, FontStyle.Regular);
        private Style numbers = new TextStyle(Brushes.Magenta, Brushes.Transparent, FontStyle.Regular);
        //C++ Specific
        private Style includes = new TextStyle(Brushes.Brown, Brushes.Transparent, FontStyle.Regular);
        private Style commonCppTypes = new TextStyle(Brushes.DarkBlue, Brushes.Transparent, FontStyle.Regular);

        public Editor()
        {
            
        }

        public void SetLang(string lang)
        {
            TextChanged -= HaxeEditor_TextChanged;
            TextChanged -= CPPEditor_TextChanged;

            switch (lang)
            {
                case "haxe":
                    Language = FastColoredTextBoxNS.Language.Custom;
                    TextChanged += HaxeEditor_TextChanged;
                    break;
                case "cpp":
                    Language = FastColoredTextBoxNS.Language.Custom;
                    TextChanged += CPPEditor_TextChanged;
                    break;
                case "html":
                    Language = FastColoredTextBoxNS.Language.HTML;
                    break;
                case "cs":
                    Language = FastColoredTextBoxNS.Language.CSharp;
                    break;
                case "js":
                    Language = FastColoredTextBoxNS.Language.JS;
                    break;
                case "lua":
                    Language = FastColoredTextBoxNS.Language.Lua;
                    break;
                case "php":
                    Language = FastColoredTextBoxNS.Language.PHP;
                    break;
                case "sql":
                    Language = FastColoredTextBoxNS.Language.SQL;
                    break;
                case "vb":
                    Language = FastColoredTextBoxNS.Language.VB;
                    break;
                case "xml":
                    Language = FastColoredTextBoxNS.Language.XML;
                    break;
                default:
                    Language = FastColoredTextBoxNS.Language.Custom;
                    break;
            }
        }

        private void CPPEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle();
            e.ChangedRange.ClearFoldingMarkers();

            e.ChangedRange.SetFoldingMarkers("{", "}");
            e.ChangedRange.SetFoldingMarkers("#if", "#end");
            e.ChangedRange.SetFoldingMarkers("#elseif", "#end");
            e.ChangedRange.SetFoldingMarkers("#ifndef", "#end");

            e.ChangedRange.SetStyle(comments, @"//.*$", RegexOptions.Multiline | RegexCompiledOption);
            e.ChangedRange.SetStyle(comments, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexCompiledOption);
            e.ChangedRange.SetStyle(comments, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline |
                RegexOptions.RightToLeft | RegexCompiledOption);

            e.ChangedRange.SetStyle(classes, @"(class|new|struct|enum)\s+(<range>[\w_]+)", RegexCompiledOption);
            e.ChangedRange.SetStyle(strings, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'", RegexCompiledOption);
            e.ChangedRange.SetStyle(numbers, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b", RegexCompiledOption);
            e.ChangedRange.SetStyle(keywords, @"\b(alignas|alignof|and|and_eq|asm|auto|bitand|bitor|bool|break|case|catch|char|char16_t|char32_t|class|compl|const|constexpr|const_cast|continue|decltype|default|delete|do|double|dynamic_cast|else|enum|explicit|export|extern|false|float|for|friend|goto|if|inline|int|long|mutable|namespace|new|noexcept|not|not_eq|nullptr|operator|or|or_eq|private|protected|public|register|reinterpret_cast|return|short|signed|sizeof|static|static_assert|static_cast|struct|switch|template|this|thread_local|throw|true|try|typedef|typeid|typename|union|unsigned|using|virtual|void|volatile|wchar_t|while|xor|xor_eq)\b", RegexCompiledOption);
            e.ChangedRange.SetStyle(includes, @"#include (\<(.+?)\>|""(.+?)"")");
        }

        private void HaxeEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle();
            e.ChangedRange.ClearFoldingMarkers();

            e.ChangedRange.SetFoldingMarkers("{", "}");

            e.ChangedRange.SetStyle(comments, @"//.*$", RegexOptions.Multiline | RegexCompiledOption);
            e.ChangedRange.SetStyle(comments, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexCompiledOption);
            e.ChangedRange.SetStyle(comments, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline |
                RegexOptions.RightToLeft | RegexCompiledOption);

            e.ChangedRange.SetStyle(classes, @"(class|extends|new|interface|enum)\s+(<range>[\w_]+)", RegexCompiledOption);
            e.ChangedRange.SetStyle(classes, @":(<range>[\w_]+)", RegexCompiledOption);
            e.ChangedRange.SetStyle(keywords, @"\b(class|extends|var|private|public|new|function|override|static|inline|for|in|while|case|select|dynamic|null|if|else|#if|#end|#elseif|import|package|implements|never|typedef|interface|enum|return|true|false)\b", RegexCompiledOption);
            e.ChangedRange.SetStyle(meta, @"@:\w", RegexCompiledOption);
            e.ChangedRange.SetStyle(strings, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'", RegexCompiledOption);
            e.ChangedRange.SetStyle(numbers, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b", RegexCompiledOption);
        }

    }
}
