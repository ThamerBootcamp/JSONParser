using System;
using System.Collections.Generic;

namespace JsonParser
{
    public class Json_Parser
    {
        public Json_Parser()
        {
        }
        public JSONValue parse(List<Token> tokens)
        {
            JSONValue root;
            var firstToken = tokens[0];
            tokens.RemoveAt(0);

            //if(firstToken.Value == "{")
            //{
            //    root = null;//new ObjectJSONValue(tokens) //readObject(tokens);
            //}
            //else
            if (firstToken.Type=="string")
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
                throw new Exception("Parsing Error: invalid Json");
            }

            /* Jvalue root;
             * firsttoken ;
             * 
             *  if firsttoken.value = "{":
             *      root = new JObject() <== readObject(tokens){
             *      
             *  elif token.type =string:
             *      root <== new JString(token.value)
             *     
             */
            if (tokens.Count>0 || root ==null)
            {
                throw new Exception("Parsing Error: invalid Json");
            }
            return root;
        }

    }

    public abstract class JSONValue
    {
        public Object Value { get; set; }
    }


    class StringJSONValue : JSONValue
    {
        public StringJSONValue(string value)
        {
            this.Value = value;
        }
    }
    class NumberJSONValue : JSONValue
    {
        public NumberJSONValue(double value)
        {
            this.Value = value;
        }
    }
    class BooleanJSONValue : JSONValue
    {
        public BooleanJSONValue(bool value)
        {
            this.Value = value;
        }
    }

    class NullJSONValue : JSONValue
    {
        public NullJSONValue()
        {
            this.Value = "null";
        }
    }

    class ObjectJSONValue : JSONValue
    {
        public ObjectJSONValue(List<KeyValue> value)
        {
            this.Value = value;
        }
        public ObjectJSONValue(NullJSONValue value)
        {
            this.Value = value;
        }
    }
    class ArrayJSONValue : JSONValue { }

    class KeyValue
    {
        public string key;
        public JSONValue Value;
    }

}
