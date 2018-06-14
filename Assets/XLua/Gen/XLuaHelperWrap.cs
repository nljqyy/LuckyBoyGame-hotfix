#if USE_UNI_LUA
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
    public class XLuaHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateArrayInstance", _m_CreateArrayInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateListInstance", _m_CreateListInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateDictionaryInstance", _m_CreateDictionaryInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateActionDelegate", _m_CreateActionDelegate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGenericListType", _m_GetGenericListType_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGenericDictionaryType", _m_GetGenericDictionaryType_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGenericActionType", _m_GetGenericActionType_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaCallCSharp", _g_get_LuaCallCSharp);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CSharpCallLua", _g_get_CSharpCallLua);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaCallCSharp", _s_set_LuaCallCSharp);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CSharpCallLua", _s_set_CSharpCallLua);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "XLuaHelper does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateArrayInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type itemType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    int itemCount = LuaAPI.xlua_tointeger(L, 2);
                    
                        System.Array __cl_gen_ret = XLuaHelper.CreateArrayInstance( itemType, itemCount );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateListInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type itemType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    
                        System.Collections.IList __cl_gen_ret = XLuaHelper.CreateListInstance( itemType );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateDictionaryInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type keyType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    System.Type valueType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        System.Collections.IDictionary __cl_gen_ret = XLuaHelper.CreateDictionaryInstance( keyType, valueType );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateActionDelegate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type type = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    System.Type[] paramTypes = translator.GetParams<System.Type>(L, 3);
                    
                        System.Delegate __cl_gen_ret = XLuaHelper.CreateActionDelegate( type, methodName, paramTypes );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGenericListType_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type itemType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    
                        System.Type __cl_gen_ret = XLuaHelper.GetGenericListType( itemType );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGenericDictionaryType_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type keyType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    System.Type valueType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        System.Type __cl_gen_ret = XLuaHelper.GetGenericDictionaryType( keyType, valueType );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGenericActionType_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type[] parmsType = translator.GetParams<System.Type>(L, 1);
                    
                        System.Type __cl_gen_ret = XLuaHelper.GetGenericActionType( parmsType );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaCallCSharp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, XLuaHelper.LuaCallCSharp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CSharpCallLua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, XLuaHelper.CSharpCallLua);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaCallCSharp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    XLuaHelper.LuaCallCSharp = (System.Collections.Generic.List<System.Type>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<System.Type>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CSharpCallLua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    XLuaHelper.CSharpCallLua = (System.Collections.Generic.List<System.Type>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<System.Type>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
