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
    public class UIMovePageWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UIMovePage);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 3, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCreate", _m_OnCreate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ShowPos", _g_get_ShowPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hidePage", _g_get_hidePage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "listPang", _g_get_listPang);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "listPang", _s_set_listPang);
            
			Utils.RegisterLazyFunc(L, Utils.METHOD_IDX, "<>xLuaBaseProxy_OnCreate", type, LazyMemberTypes.Method, false);
            Utils.RegisterLazyFunc(L, Utils.METHOD_IDX, "<>xLuaBaseProxy_OnEnter", type, LazyMemberTypes.Method, false);
            
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NAME", UIMovePage.NAME);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UIMovePage __cl_gen_ret = new UIMovePage();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UIMovePage constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCreate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnCreate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnEnter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ShowPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.ShowPos);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hidePage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.hidePage);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_listPang(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.listPang);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_listPang(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMovePage __cl_gen_to_be_invoked = (UIMovePage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.listPang = (System.Collections.Generic.List<GameEntity>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<GameEntity>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
