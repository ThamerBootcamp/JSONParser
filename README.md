
<div dir="rtl"> 

# وصف المشروع 


المدخلات :

البرنامج يستقبل مدخل من نوع String ويقوم بتحليله باستخدام tokenizer  ليتحقق من صحة توافقه مع صيغة JSON. 

المخرجات : 

البرنامج يعرض البيانات الناجحه بصيغة JSON وكذلك يتيح استدعاء قيمة معينة باستخدام index . 
في حال حدوث خطا في صيغة String  يعرض رسالة الخطأ وسطر حدوثه.




# التصميم
<div dir="ltr"> 

### Data flow
![Data flow](https://mermaid.ink/img/eyJjb2RlIjoiZ3JhcGggXG4gICAgQVtJbnB1dF0gLS0-IEIoVG9rZW5pemVyKVxuICAgIEIgLS0-fFRva2VuIExpc3R8IEMoUGFyc2VyKVxuICAgIEMgLS0-fHBhcnNlfERbSlNPTlZhbHVlXVxuIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRhcmsifSwidXBkYXRlRWRpdG9yIjpmYWxzZX0)


``` 

Input.cs (Starter Code: with some slight changes) : 

* Contains functions to deal with a String value 


```

``` 

Tokenizer.cs : 

* It uses the functions inside of Input.cs to generate tokens based on a custom handlers written and tested by the team: 
   - WhiteSpaceTokenizer 
   - NewLineTokenizer 
   - JSymbolsTokenizer ({}[],)
   - KeywordsTokenizer (false,true,null)
   - StringTokenizer 
   - NumberTokenizer

All handlers written to match the JSON parser convention 


```

[JSON Rules](https://www.json.org/json-en.html)


### Parser Class Diagram 

![Class Diagram](https://i.ibb.co/7z7C1Vk/ey-Jjb2-Rl-Ijoi-Y2xhc3-NEa-WFncm-Ft-XG4g-ICAg-Y2xhc3-Mg-Sn-Nvbl9-QYXJz-ZXJ7-XG4g-ICAg-ICArc-GFyc2-Uo.jpg)


```
JsonParser.cs (Main Design):
 
* It uses the generated list of tokens from the "Tokenizer" by the reference "ref".

* Classes: 
    - JsonParser : 
       ~ Controls the flow and decide which JsonValue should the token be via using the method (parse)
    - KeyValue : Helper class
          [[Inheritance]]
    - JSONValue (abstract & parent): 
       ~ Children: 
          - StringJSONValue 
          - NumberJSONValue 
          - BooleanJSONValue
          - NullJSONValue
          - ObjectJSONValue
          - ArrayJSONValue 


```


```

Program.cs (main)

* To run and test. 
```

</div>

# تصميم آخر

تصميم آخر يعتمد على سهولة الوصول للمتغيرات باستخدام Method Chaining : 

https://github.com/ThamerBootcamp/JSON-Parser

- سبب وجود أكثر من تصميم ناتج من نقاش المجموعة وفهمهم للمشكلة من جهة أخرى. 


# أعضاء القروب 

1- ثامر مشني 

2- عبدالمجيد الميموني 

3- أنس الحمود

4- منصور آل دندور 

</div>
