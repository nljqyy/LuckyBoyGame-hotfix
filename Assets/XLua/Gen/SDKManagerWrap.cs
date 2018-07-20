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
    public class SDKManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SDKManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 22, 16, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Current", _m_Current);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsCanPlay", _m_IsCanPlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "isTabke", _m_isTabke);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPayStatus", _m_GetPayStatus);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetQR_Code", _m_GetQR_Code);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RecordTimes", _m_RecordTimes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Speak", _m_Speak);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Wave", _m_Wave);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WonDoll", _m_WonDoll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetProbability", _m_GetProbability);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CustomQuit", _m_CustomQuit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Light", _m_Light);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoSendPresent", _m_AutoSendPresent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AndroidCall", _m_AndroidCall);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QRCodeCall", _m_QRCodeCall);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetProbabilityCall", _m_GetProbabilityCall);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TimeFun", _m_TimeFun);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetVoiceForType", _m_GetVoiceForType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartCatchBoy", _m_StartCatchBoy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEnd", _m_SetEnd);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCaught", _m_SetCaught);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AppQuit", _m_AppQuit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "isCaught", _g_get_isCaught);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isHas", _g_get_isHas);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isTakeAway", _g_get_isTakeAway);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "caughtTime", _g_get_caughtTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isPaySucess", _g_get_isPaySucess);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isEnd", _g_get_isEnd);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gameXP", _g_get_gameXP);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "randomQuXian", _g_get_randomQuXian);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "probability", _g_get_probability);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "winningTimes", _g_get_winningTimes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "carwBasicCount", _g_get_carwBasicCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startCarwTime", _g_get_startCarwTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "getCode", _g_get_getCode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gameStatus", _g_get_gameStatus);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "checkProperty", _g_get_checkProperty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isOpenPay", _g_get_isOpenPay);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "gameXP", _s_set_gameXP);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "randomQuXian", _s_set_randomQuXian);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startCarwTime", _s_set_startCarwTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gameStatus", _s_set_gameStatus);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "checkProperty", _s_set_checkProperty);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isOpenPay", _s_set_isOpenPay);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Instance", _s_set_Instance);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					SDKManager __cl_gen_ret = new SDKManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SDKManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Current(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.AndroidJavaObject __cl_gen_ret = __cl_gen_to_be_invoked.Current(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsCanPlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.IsCanPlay(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isTabke(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.isTabke(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPayStatus(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.GetPayStatus(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetQR_Code(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.UI.RawImage r = (UnityEngine.UI.RawImage)translator.GetObject(L, 2, typeof(UnityEngine.UI.RawImage));
                    
                    __cl_gen_to_be_invoked.GetQR_Code( r );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecordTimes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int num = LuaAPI.xlua_tointeger(L, 2);
                    bool isSuccess = LuaAPI.lua_toboolean(L, 3);
                    
                    __cl_gen_to_be_invoked.RecordTimes( num, isSuccess );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Speak(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string msg = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Speak( msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Wave(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int num = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Wave( num );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WonDoll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool state = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.WonDoll( state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetProbability(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.GetProbability(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CustomQuit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CustomQuit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Light(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool n = LuaAPI.lua_toboolean(L, 2);
                    int num = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.Light( n, num );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoSendPresent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.AutoSendPresent(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AndroidCall(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string result = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.AndroidCall( result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QRCodeCall(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string result = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.QRCodeCall( result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetProbabilityCall(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string result = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.GetProbabilityCall( result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TimeFun(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<MyFuncPerSecond>(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    float deltime = (float)LuaAPI.lua_tonumber(L, 2);
                    float spacetime = (float)LuaAPI.lua_tonumber(L, 3);
                    MyFuncPerSecond func = translator.GetDelegate<MyFuncPerSecond>(L, 4);
                    System.Action aciton = translator.GetDelegate<System.Action>(L, 5);
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.TimeFun( deltime, spacetime, func, aciton );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<MyFuncPerSecond>(L, 4)) 
                {
                    float deltime = (float)LuaAPI.lua_tonumber(L, 2);
                    float spacetime = (float)LuaAPI.lua_tonumber(L, 3);
                    MyFuncPerSecond func = translator.GetDelegate<MyFuncPerSecond>(L, 4);
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.TimeFun( deltime, spacetime, func );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float deltime = (float)LuaAPI.lua_tonumber(L, 2);
                    float spacetime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.TimeFun( deltime, spacetime );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to SDKManager.TimeFun!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVoiceForType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    VoiceType type;translator.Get(L, 2, out type);
                    
                        System.Collections.Generic.List<ExcelTableEntity> __cl_gen_ret = __cl_gen_to_be_invoked.GetVoiceForType( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartCatchBoy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.StartCatchBoy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEnd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.SetEnd(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCaught(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.SetCaught(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppQuit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.AppQuit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isCaught(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isCaught);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isHas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isHas);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isTakeAway(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isTakeAway);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_caughtTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.caughtTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isPaySucess(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isPaySucess);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isEnd(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isEnd);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gameXP(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gameXP);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_randomQuXian(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.randomQuXian);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_probability(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.probability);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_winningTimes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.winningTimes);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_carwBasicCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.carwBasicCount);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startCarwTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.startCarwTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_getCode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.getCode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gameStatus(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gameStatus);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_checkProperty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.checkProperty);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isOpenPay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isOpenPay);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, SDKManager.Instance);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gameXP(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gameXP = (GameEntity)translator.GetObject(L, 2, typeof(GameEntity));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_randomQuXian(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.randomQuXian = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startCarwTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.startCarwTime = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gameStatus(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gameStatus = (GameStatus)translator.GetObject(L, 2, typeof(GameStatus));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_checkProperty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.checkProperty = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isOpenPay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SDKManager __cl_gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.isOpenPay = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    SDKManager.Instance = (SDKManager)translator.GetObject(L, 1, typeof(SDKManager));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
