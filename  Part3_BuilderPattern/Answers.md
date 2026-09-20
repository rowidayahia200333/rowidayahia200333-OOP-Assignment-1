1- when we call the constructor for creat an object we will make a long constructor (parameter) 
ف هيصعب القراءة كمان برا لما اجي احط القيم في ال constructor هتلغبط وابقي مش عارفة احط انهو نوع الاول فيهم 

2- انتهاك مبدأ المسؤولية الواحدة (Single Responsibility Principle - SRP): الكلاس تحول إلى ما يُعرف بـ God Object أو Large Class لأنه يتحمل مسؤوليات كثيرة (بيانات الفاتورة، بيانات العميل، عنوان الفوترة، عنوان الشحن، وبيانات الدفع).
غياب التجميع المفاهيمي (Lack of Encapsulation / Value Objects): الخصائص مقسمة مفاهيمياً إلى مجموعات (Addresses و Order Info)، لكنها مفككة داخل الكلاس. التصميم الأفضل يتطلب فصل العناوين في كلاس مستقل مثل Address وإعادة استخدامه لـ
ف بنستخدم ال Builder design pattern عشان يبقي اوضح واسهل في القراية 


Question:
why is this composed version better than the single big builder from Task 3.2 ?

answer:
1-Single responsibility: كل Builder بيبقي مسؤل عن كلاس معين جزء من المعلومات اللي مرتبطين ببعض اكتر مع بعض زي ان ال addressBuilder مسؤول بس عن street , city اما ال invoiceBuilder مسؤول عن المعلومات الشخصية name email and phone of customer 

2- Independent validation: ايوة يقدر يبقي عنده العنوان كامل من غير اي تدخل من الكلاس الاب خالص ويعمل validate علي عادي هو مستقل بال validate بتاعة في الكلاس بتاعة مش بيحتاج تدخل الاب 

3- كنت هكرر نفس ال validate علي ال adresss مثلا تاني مرتين في ال billing و ال shipping وده يسببلي اني انتهك مبدأ ال DRY consept 

4- في ال constructor الاول بتاع كلاس invoice كان معقد وفيه بياخد parameters كتير ف كان صعب القراية وصعب علي المستخدم اللي بيدخل بيانات فيه ان يحفظ ويعرف مكان كل داتا فين واية النوع اللي المفروض يدخله هنا الاول 
لكن لما اخليهم في كلاس لوحدهم تاني يرجعلي ال this يعني ال calling object تاني لكل ميثود فيهم ده يخليه اسهل في القراية بكتير واسرع واسهل في اني ادخل البيانات من برا الكلاس وانا بستدعيها بسهولة من غير ما احفظ ادخل نوع اية ااول 
                        