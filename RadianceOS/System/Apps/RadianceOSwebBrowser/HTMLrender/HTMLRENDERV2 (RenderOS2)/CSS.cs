using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webkerneltest.UTILITIES;

namespace webkerneltest.HTMLRENDERV2
{
    public class CssParser
    {

        public static CSSRules Empty()
        {

            return new CSSRules() { rawrules = new Dictionary<string, Dictionary<string, string>>() };
        }

        public static CSSRules Parse(string css)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var rules = css.Split('}');

            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule))
                    continue;

                var parts = rule.Split('{');
                var selector = parts[0].Trim();
                var declarations = parts[1].Split(';');

                var properties = new Dictionary<string, string>();
                foreach (var declaration in declarations)
                {
                    if (string.IsNullOrWhiteSpace(declaration))
                        continue;

                    var declarationParts = declaration.Split(':');
                    properties[declarationParts[0].Trim()] = declarationParts[1].Trim();
                }

                result[selector] = properties;
            }

            return new CSSRules() {rawrules=result};
        }
        public static CSSRules Update(string css,CSSRules Rules)
        {
            var result = Rules.rawrules;
            var rules = css.Split('}');

            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule))
                    continue;

                var parts = rule.Split('{');
                var selector = parts[0].Trim();
                var declarations = parts[1].Split(';');

                var properties = new Dictionary<string, string>();
                foreach (var declaration in declarations)
                {
                    if (string.IsNullOrWhiteSpace(declaration))
                        continue;

                    var declarationParts = declaration.Split(':');
                    properties[declarationParts[0].Trim()] = declarationParts[1].Trim();
                }

                result[selector] = properties;
            }

            return new CSSRules() { rawrules = result };
        }
    }

    public class CSSRules
    {

        public Dictionary<string, Dictionary<string, string>> rawrules;

        public Color BackgroundColor(string selector)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("background-color"))
                {
                    return HexToArgbConverter.HexToArgb(rawrules[selector]["background-color"]);
                }

            }

            return Color.White;

        }
        public Color TextColor(string selector)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("color"))
                {
                    return HexToArgbConverter.HexToArgb(rawrules[selector]["color"]);
                }

            }

            return Color.Black;

        }

        public bool TextAlign(string selector)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("text-align"))
                {
                    if (rawrules[selector]["text-align"] == "center")
                    {
                        return true;
                    }
                }

            }

            return false;

        }

        public Color GetUknownAsColor(string selector,string key)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("key"))
                {
                    return HexToArgbConverter.HexToArgb(rawrules[selector]["key"]);
                }

            }

            return Color.White;

        }
        public string GetUknownAsString(string selector, string key)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("key"))
                {
                    return rawrules[selector]["key"];
                }

            }

            return "";

        }
        public int GetUknownAsInt(string selector, string key)
        {

            if (rawrules.ContainsKey(selector))
            {

                if (rawrules[selector].ContainsKey("key"))
                {
                    return int.Parse(rawrules[selector]["key"]);
                }

            }

            return 0;

        }

    }

}
