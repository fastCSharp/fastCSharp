这个版本是fastCSharp的第五个部分，主要包括以下基础类库：
1、基于.net元数据的代码生成器，fastCSharp.setup.cSharp.coder用于解析模板生成中间程序fastCSharp.cSharper.cs。
2、基于缓存查询模式的ORM代码生成实例，自定义配置类是fastCSharp.setup.cSharp.sqlTable，同时支持反射模式fastCSharp.setup.cSharp.sqlTable.sqlTool<valueType>。
3、数据类快速序列化代码生成实例，自定义配置类是fastCSharp.setup.cSharp.serialize，同时支持反射模式fastCSharp.setup.cSharp.serialize.dataSerialize.Get fastCSharp.setup.cSharp.serialize.streamSerialize.Get fastCSharp.setup.cSharp.serialize.deSerialize.Get。
4、基本TCP调用代码生成实例，自定义配置类是fastCSharp.setup.cSharp.tcpCall，支持泛型，支持跨类（只能支持单例），不支持反射模式。
5、基本TCP服务代码生成实例，自定义配置类是fastCSharp.setup.cSharp.tcpServer，支持泛型，不支持跨类，不支持反射模式。
6、快速json处理代码生成实例，自定义配置类是fastCSharp.setup.cSharp.ajax，同时支持反射模式fastCSharp.setup.cSharp.ajax.toJson.Get + fastCSharp.setup.cSharp.ajax.parseJson.Get。
7、支持常用数据集合操作的链式编程体验的扩展方法(小写字母开始的方法)，无需判断当前对象是否为null，用于取代Linq to Object。所有数据操作，0个元素的集合可能会返回null，你可以调用.notNull()转换为空数组。
list用于取代System.Collections.Generic.List，collection用于取代System.Collections.Generic.Queue和System.Collections.Generic.Stack，支持非安全操作(Unsafer)以及泛型实参化扩展，提升程序运行效率。
注意：由于某些编译器的新特性支持，请使用vs2010及以上版本的IDE。
要编译64位程序并需要优化内存操作的，可以设置条件编译符号 CPU64。