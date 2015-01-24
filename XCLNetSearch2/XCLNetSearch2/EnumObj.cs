using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace XCLNetSearch
{
    internal class EnumObj
    {
        #region 将枚举转为list
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        public static List<EnumObj> GetList(Type type)
        {
            if (!type.IsEnum)
            {    //不是枚举的要报错
                throw new InvalidOperationException();
            }
            //建立列表
            List<EnumObj> list = new List<EnumObj>();
            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            System.Reflection.FieldInfo[] fields = type.GetFields();
            //检索所有字段
            foreach (FieldInfo field in fields)
            {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum)
                {
                    EnumObj obj = new EnumObj();
                    // 通过字段的名字得到枚举的值
                    // 枚举的值如果是long的话，ToChar会有问题，但这个不在本文的讨论范围之内
                    obj.Value = ((int)type.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    obj.Text = field.Name;
                    list.Add(obj);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 判断数字是否属于该枚举
        /// </summary>
        public static bool IsExistEnumValue(int v, Type type)
        {
            bool flag = false;
            List<EnumObj> lst = GetList(type);
            foreach (EnumObj m in lst)
            {
                if (m.Value == v.ToString())
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}
