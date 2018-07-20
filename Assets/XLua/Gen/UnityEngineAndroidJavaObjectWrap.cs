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
    public class UnityEngineAndroidJavaObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.AndroidJavaObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Call", _m_Call);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallStatic", _m_CallStatic);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRawObject", _m_GetRawObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRawClass", _m_GetRawClass);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) >= 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3)))
				{
					string className = LuaAPI.lua_tostring(L, 2);
					object[] args = translator.GetParams<object>(L, 3);
					
					UnityEngine.AndroidJavaObject __cl_gen_ret = new UnityEngine.AndroidJavaObject(className, args);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AndroidJavaObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AndroidJavaObject __cl_gen_to_be_invoked = (UnityEngine.AndroidJavaObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Call(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AndroidJavaObject __cl_gen_to_be_invoked = (UnityEngine.AndroidJavaObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    __cl_gen_to_be_invoked.Call( methodName, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallStatic(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AndroidJavaObject __cl_gen_to_be_invoked = (UnityEngine.AndroidJavaObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    __cl_gen_to_be_invoked.CallStatic( methodName, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRawObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AndroidJavaObject __cl_gen_to_be_invoked = (UnityEngine.AndroidJavaObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.IntPtr __cl_gen_ret = __cl_gen_to_be_invoked.GetRawObject(  );
                        LuaAPI.lua_pushlightuserdata(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRawClass(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AndroidJavaObject __cl_gen_to_be_invoked = (UnityEngine.AndroidJavaObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.IntPtr __cl_gen_ret = __cl_gen_to_be_invoked.GetRawClass(  );
                        LuaAPI.lua_pushlightuserdata(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
