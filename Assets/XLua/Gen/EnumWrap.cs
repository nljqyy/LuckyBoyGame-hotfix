﻿#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    
    public class SystemReflectionBindingFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(System.Reflection.BindingFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(System.Reflection.BindingFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(System.Reflection.BindingFlags), L, null, 21, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", System.Reflection.BindingFlags.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreCase", System.Reflection.BindingFlags.IgnoreCase);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DeclaredOnly", System.Reflection.BindingFlags.DeclaredOnly);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Instance", System.Reflection.BindingFlags.Instance);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Static", System.Reflection.BindingFlags.Static);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Public", System.Reflection.BindingFlags.Public);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NonPublic", System.Reflection.BindingFlags.NonPublic);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FlattenHierarchy", System.Reflection.BindingFlags.FlattenHierarchy);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InvokeMethod", System.Reflection.BindingFlags.InvokeMethod);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CreateInstance", System.Reflection.BindingFlags.CreateInstance);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GetField", System.Reflection.BindingFlags.GetField);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SetField", System.Reflection.BindingFlags.SetField);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GetProperty", System.Reflection.BindingFlags.GetProperty);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SetProperty", System.Reflection.BindingFlags.SetProperty);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PutDispProperty", System.Reflection.BindingFlags.PutDispProperty);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PutRefDispProperty", System.Reflection.BindingFlags.PutRefDispProperty);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ExactBinding", System.Reflection.BindingFlags.ExactBinding);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SuppressChangeType", System.Reflection.BindingFlags.SuppressChangeType);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OptionalParamBinding", System.Reflection.BindingFlags.OptionalParamBinding);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreReturn", System.Reflection.BindingFlags.IgnoreReturn);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(System.Reflection.BindingFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSystemReflectionBindingFlags(L, (System.Reflection.BindingFlags)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreCase"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.IgnoreCase);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DeclaredOnly"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.DeclaredOnly);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Instance"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.Instance);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Static"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.Static);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Public"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.Public);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NonPublic"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.NonPublic);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FlattenHierarchy"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.FlattenHierarchy);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InvokeMethod"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.InvokeMethod);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CreateInstance"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.CreateInstance);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GetField"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.GetField);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SetField"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.SetField);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GetProperty"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.GetProperty);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SetProperty"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.SetProperty);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PutDispProperty"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.PutDispProperty);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PutRefDispProperty"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.PutRefDispProperty);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ExactBinding"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.ExactBinding);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SuppressChangeType"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.SuppressChangeType);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OptionalParamBinding"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.OptionalParamBinding);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreReturn"))
                {
                    translator.PushSystemReflectionBindingFlags(L, System.Reflection.BindingFlags.IgnoreReturn);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for System.Reflection.BindingFlags!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for System.Reflection.BindingFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}