using System;
using System.Collections.Generic;

namespace JsonParser
{
    class MainClass
    {
        static void Main(string[] args)
        {
            /******       Start of Tokenizer test          *******/

            Input input = new Input(@"{
                                        ""data"": ""Click Here"",
                                        ""size"": 1,
                                        ""style"": ""bold"",
                                        ""name"": ""text1"",
                                        ""hOffset"": ""one"",
                                        ""vOffset"": ""two"",
                                        ""alignment"": ""center"",
                                        ""onMouseUp"": ""sun1.opacity = (sun1.opacity / v);"",
                                        ""More"": {
                                                ""data"": ""Click Here"",
                                                ""size"": 36,
                                                ""style"": false
                                                }   
                                    }");
            Input input2 = new Input(@"{
                                        ""data"": ""Click Here"",
                                        ""size"": 36,
                                        ""style"": false
                                    }");
            Input input3 = new Input(@"null");
            Input input4 = new Input(@"""name""");
            Input input5 = new Input(@"{ ""key here"" : ""value there"" , ""key2"" : ""value2""}");
            Input input6 = new Input(@"{ ""key here"" : { ""key here"" : ""value there""} }");

            Tokenizer t = new Tokenizer(input, new Tokenizable[] {
                new StringTokenizer(),
                new KeywordsTokenizer(new List<string>
                {
                    "true","false","null"
                }),
                new NumberTokenizer(),
                new WhiteSpaceTokenizer(true),
                new JSymbolsTokenizer('{',"opening curly braces"),
                new JSymbolsTokenizer('}',"closing curly braces"),
                new JSymbolsTokenizer('[',"opening square bracket"),
                new JSymbolsTokenizer('[',"closing square bracket"),
                new JSymbolsTokenizer(':',"colon"),
                new JSymbolsTokenizer(',',"comma"),
            });

            List<Token> tokens = new List<Token>();

            Token token = t.tokenize();
            while (token != null)
            {
                tokens.Add(token);
                Console.WriteLine(token.Value + " ---> " + token.Type);
                token = t.tokenize();
            }
            Console.WriteLine("\nTokenizer done!\n");
            /******       End of Tokenizer test          *******/

            /******       Start of Parser test          *******/

            //while (tokens.Count>0)
            //{
            //    var item = tokens[0];
            //    tokens.RemoveAt(0);
            //    Console.WriteLine(item.Value + " ---> " + item.Type);
            //}

            JSONValue json = JsonParser.Json_Parser.parse(ref tokens);

            Console.WriteLine(json.print());

        }
    }
}

//string testCase = "true  false \"anas\" null    -0 0.423 -0.23  0.22e4 0.22e+4 0.22e-4 -0.22e4 -0.22e+4 -0.22e-4 0. 8468";
//string tc = "3224 -3231  13.31 -3242.32   2E+3 2E3 2E-3 265e+324 265e324 265e-324 -2E+3 -2E3 -2E-3 -265e+324 -265e324 -265e-324";
//Tokenizer t = new Tokenizer(new Input(testCase), new Tokenizable[] {
//    new StringTokenizer(),
//    new KeywordsTokenizer(new List<string>
//    {
//        "true","false","null"
//    }),
//    new NumberTokenizer(),
//    new WhiteSpaceTokenizer(),

//}); 
//Token token = t.tokenize();

//while (token != null)
//{
//    Console.WriteLine(token.Value + " ---> " + token.Type);
//    token = t.tokenize();
//}
//Console.WriteLine(typeof());
//JSONValue testParse = JSON.parse("{\"name\":\"thamer\"}");
//Console.WriteLine(testParse.Value.GetType());
//testParse.va
//Console.WriteLine(testParse);
