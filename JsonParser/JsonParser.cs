﻿using System;
using System.Collections.Generic;

namespace JsonParser
{
    public class Json_Parser
    {
        public static JSONValue parse(ref List<Token> tokens)
        {
            if (tokens.Count== 0)
            {
                throw new Exception("Error: No Data Available");
            }
            JSONValue root;
            var firstToken = tokens[0];
            tokens.RemoveAt(0);

            if (firstToken.Value == "{")
            {
                root = ReadObject(ref tokens);
            }
            else if (firstToken.Value == "[")
            {
                root = ReadArray(ref tokens);
            }
            else if (firstToken.Type == "string")
            {
                root = new StringJSONValue(firstToken.Value);
            }
            else if (firstToken.Type == "number")
            {
                try
                {
                    root = new NumberJSONValue(Convert.ToDouble(firstToken.Value));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else if (firstToken.Type == "boolean")
            {
                try
                {
                    root = new BooleanJSONValue(Convert.ToBoolean(firstToken.Value));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (firstToken.Type == "null")
            {
                root = new NullJSONValue();
            }
            else
            {
                throw new Exception("Parsing Error: invalid Json at line number: " + tokens[0].LineNumber);
            }
            List<string> sep = new List<string> { ",", ":","]" ,"}"};
            if (tokens.Count > 0 && root != null && !sep.Contains(tokens[0].Value)) 
            {
                throw new Exception("Parsing Error: invalid Json at line number: " + tokens[0].LineNumber);
            }

            return root;
        }

        public static ArrayJSONValue ReadArray(ref List<Token> tokens)
        {
            List<JSONValue> body = new List<JSONValue>();
            while (tokens.Count > 0)
            {
                //Token currentToken = tokens[0];

                if (tokens[0].Value == ",") {
                    tokens.RemoveAt(0);
                    body.Add(parse(ref tokens));
                }
                else if (tokens[0].Value == "]")
                {
                    tokens.RemoveAt(0);
                    return new ArrayJSONValue(body);
                }
                else 
                {
                    body.Add(parse(ref tokens));
                }

            }
            throw new Exception("Parsing Error: invalid Json object at line number: " + tokens[0].LineNumber);
        }
           
        public static ObjectJSONValue ReadObject(ref List<Token> tokens)
        {
            List<KeyValue> body = new List<KeyValue>();

            while (tokens.Count > 0)
            {
                //Token currentToken = tokens[0];

                if (tokens[0].Type == "string")
                {
                    body.Add(ReadKeyValue(ref tokens));
                }       
                else if (tokens[0].Value == ",")
                {
                    tokens.RemoveAt(0);
                    //foreach (var item in  tokens)
                    //{
                    //    Console.Write(item.Value);
                    //}
                    body.Add(ReadKeyValue(ref tokens));
                }
                else if (tokens[0].Value == "}")
                {
                    tokens.RemoveAt(0);
                    return new ObjectJSONValue(body);
                }
                else
                {
                    throw new Exception("Parsing Error: invalid Json object at line number: " + tokens[0].LineNumber);
                }

            }
            throw new Exception("Parsing Error: invalid Json object at line number: " + tokens[0].LineNumber);
        }

        public static KeyValue ReadKeyValue(ref List<Token> tokens)
        {
            KeyValue row = new KeyValue();
            
            //Token currentToken = tokens[0];

            if (tokens[0].Type == "string")
            {
                row.Key = tokens[0].Value;

                tokens.RemoveAt(0);
                //tokens[0] = tokens[0];

                if (tokens[0].Value == ":")
                {
                    tokens.RemoveAt(0);
                    row.Value = Json_Parser.parse(ref tokens);
                }
                else
                {
                    throw new Exception("Parsing Error: missing colon after Key, at line number: " + tokens[0].LineNumber);
                }

                return row;
            }
            else
            {
                throw new Exception("Parsing Error: Token is not a Key, at line number: " + tokens[0].LineNumber);
            }
        }
    }
    public abstract class JSONValue
    {
        public Object Value { get; set; }
        public abstract string print();
        public abstract Object getValue();

    }


    class StringJSONValue : JSONValue
    {
        public new string Value { get; set; }

        public StringJSONValue(string value)
        {
            this.Value = value;
        }

        public override Object getValue()
        {
            return (string)this.Value;            
        }

        public override string print()
        {
            return "\"" + this.Value + "\"";
        }
    }
    class NumberJSONValue : JSONValue
    {
        public new double Value { get; set; }

        public NumberJSONValue(double value)
        {
            this.Value = value;
        }

        public override Object getValue()
        {
            return (double)this.Value;
        }

        public override string print()
        {
            return this.Value.ToString();
        }
    }
    class BooleanJSONValue : JSONValue
    {
        public new bool Value { get; set; }

        public BooleanJSONValue(bool value)
        {
            this.Value = value;
        }

        public override Object getValue()
        {
            return (bool)this.Value;
        }

        public override string print()
        {
            return this.Value.ToString();
        }
    }

    class NullJSONValue : JSONValue
    {
        public NullJSONValue()
        {
            this.Value = "null";
        }

        public override Object getValue()
        {
            return (string) this.Value;
        }

        public override string print()
        {
            return (string)this.Value;
        }

    }

    public class ObjectJSONValue : JSONValue
    {
        public new List<KeyValue> Value { get; set; }

        public ObjectJSONValue(List<KeyValue> value)
        {
            this.Value = value;
        }

        public override string print()
        {
            if (Value.Count == 0)
            {
                return "{}";
            }

            string print = "{\n";
            //foreach (var item in Value)
            //{
            for (int i = 0; i < Value.Count; i++)
            {
                if(i == Value.Count -1)
                    print += " \"" + Value[i].Key + "\" : " + Value[i].Value.print() + "\n";

                else print += " \"" + Value[i].Key + "\" : " + Value[i].Value.print() + ",\n";

            }

            print += "}";

            return print;
            //throw new NotImplementedException();

        }

        public override Object getValue()
        {
            return (List<KeyValue>)this.Value;
        }
    }
    public class ArrayJSONValue : JSONValue
    {
        public new List<JSONValue> Value { get; set; }

        public ArrayJSONValue(List<JSONValue> value)
        {
            this.Value = value;

        }
        public override string print()
        {
            if(this.Value.Count == 0)
            {
                return "[]";
            }
            string print = "[ ";
            for (int i = 0; i < Value.Count; i++)
            {
                if (i == Value.Count - 1)
                    print +=  Value[i].print() + "\n";

                else print += Value[i].print() + ",\n";
            }
            print += " ]";

            return print;
        }

        public override Object getValue()
        {
            return (List<JSONValue>)this.Value;
        }
    }

    public class KeyValue
    {
        public string Key;
        public JSONValue Value;

        public string getKey()
        {
            return this.Key;
        }
        public Object getValue()
        {
            return this.Value.getValue();
        }

    }

    
}
