#region FASTCSHARPDEFINE
#endregion FASTCSHARPDEFINE
using System;

#if DOTNET35
namespace fastCSharp.sql.expression
#else
namespace System.Linq.Expressions
#endif
{
    /// <summary>
    /// 表达式类型
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// 二元+
        /// </summary>
        Add,
        /// <summary>
        /// 二元+
        /// </summary>
        AddChecked,
        /// <summary>
        /// 二元&
        /// </summary>
        And,
        /// <summary>
        /// 二元&&
        /// </summary>
        AndAlso,
        ArrayLength,
        ArrayIndex,
        /// <summary>
        /// 函数调用
        /// </summary>
        Call,
        Coalesce,
        /// <summary>
        /// 条件表达式
        /// </summary>
        Conditional,
        /// <summary>
        /// 常量表达式
        /// </summary>
        Constant,
        /// <summary>
        /// 类型转换
        /// </summary>
        Convert,
        /// <summary>
        /// 类型转换
        /// </summary>
        ConvertChecked,
        /// <summary>
        /// 二元/
        /// </summary>
        Divide,
        /// <summary>
        /// 二元==
        /// </summary>
        Equal,
        /// <summary>
        /// 二元^
        /// </summary>
        ExclusiveOr,
        /// <summary>
        /// 二元>
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 二元>=
        /// </summary>
        GreaterThanOrEqual,
        Invoke,
        /// <summary>
        /// lambda表达式
        /// </summary>
        Lambda,
        /// <summary>
        /// 二元<<
        /// </summary>
        LeftShift,
        /// <summary>
        /// 二元<
        /// </summary>
        LessThan,
        /// <summary>
        /// 二元<=
        /// </summary>
        LessThanOrEqual,
        ListInit,
        /// <summary>
        /// 成员访问
        /// </summary>
        MemberAccess,
        MemberInit,
        /// <summary>
        /// 二元%
        /// </summary>
        Modulo,
        /// <summary>
        /// 二元*
        /// </summary>
        Multiply,
        /// <summary>
        /// 二元*
        /// </summary>
        MultiplyChecked,
        /// <summary>
        /// 一元-
        /// </summary>
        Negate,
        /// <summary>
        /// 一元+
        /// </summary>
        UnaryPlus,
        /// <summary>
        /// 一元-
        /// </summary>
        NegateChecked,
        New,
        NewArrayInit,
        NewArrayBounds,
        /// <summary>
        /// 一元!
        /// </summary>
        Not,
        /// <summary>
        /// 二元!=
        /// </summary>
        NotEqual,
        /// <summary>
        /// 二元|
        /// </summary>
        Or,
        /// <summary>
        /// 二元||
        /// </summary>
        OrElse,
        /// <summary>
        /// 参数表达式
        /// </summary>
        Parameter,
        /// <summary>
        /// 二元**
        /// </summary>
        Power,
        Quote,
        /// <summary>
        /// 二元>>
        /// </summary>
        RightShift,
        /// <summary>
        /// 二元-
        /// </summary>
        Subtract,
        /// <summary>
        /// 二元-
        /// </summary>
        SubtractChecked,
        TypeAs,
        TypeIs,
        Assign,
        Block,
        DebugInfo,
        Decrement,
        Dynamic,
        Default,
        Extension,
        Goto,
        Increment,
        Index,
        Label,
        RuntimeVariables,
        Loop,
        Switch,
        Throw,
        Try,
        /// <summary>
        /// 拆箱
        /// </summary>
        Unbox,
        AddAssign,
        AndAssign,
        DivideAssign,
        ExclusiveOrAssign,
        LeftShiftAssign,
        ModuloAssign,
        MultiplyAssign,
        OrAssign,
        PowerAssign,
        RightShiftAssign,
        SubtractAssign,
        AddAssignChecked,
        MultiplyAssignChecked,
        SubtractAssignChecked,
        PreIncrementAssign,
        PreDecrementAssign,
        PostIncrementAssign,
        PostDecrementAssign,
        TypeEqual,
        OnesComplement,
        /// <summary>
        /// 真值判定
        /// </summary>
        IsTrue,
        /// <summary>
        /// 假值判定
        /// </summary>
        IsFalse,

        /// <summary>
        /// 逻辑常量
        /// </summary>
        LogicConstant,
        /// <summary>
        /// 字段访问
        /// </summary>
        FieldAccess,
        /// <summary>
        /// 属性访问
        /// </summary>
        PropertyAccess,
        /// <summary>
        /// 集合包含
        /// </summary>
        InSet,
    }
}
