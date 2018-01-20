local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
	self.btn_yingxiong = nil	
	self.btn_jiaju = nil
	self.btn_jiagong = nil
	
	self.btn_head_ButtonEx = nil
	self.btn_bag_ButtonEx = nil
	self.btn_main_ButtonEx = nil
end

function _M:OnLoaded()
	self.btn_jiaju = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiaju')
	self.btn_jiaju.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaJu, self))
	
	self.btn_jiagong = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiagong')
	self.btn_jiagong.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaGong, self))
	
	self.btn_yingxiong = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_yingxiong')
	self.btn_yingxiong.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnYingXiong, self))
	
	--self.btn_head_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_head')	
	--self.btn_head_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnHead, self))
	
	self.btn_bag_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bag')	
	self.btn_bag_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnBag, self))
	
	self.btn_hero_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_hero')	
	self.btn_hero_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnHero, self))
	
	self.btn_main_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_main')	
	self.btn_main_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnTask, self))
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	self.btn_jiaju.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnJiaJu, self))
	self.btn_jiagong.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnJiaGong, self))
	self.btn_yingxiong.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnYingXiong, self))
end

function _M:OnJiaJu()
	uimgr.OpenUI(cf_ui.build)
end

function _M:OnJiaGong()
	uimgr.OpenUI(cf_ui.workbench)
end

function _M:OnYingXiong()
	uimgr.OpenUI(cf_ui.war)
end

function _M:OnHead()
	
end

function _M:OnBag()
	uimgr.OpenSubUI(cf_ui.bag)
end

function _M:OnHero()
	uimgr.OpenSubUI(cf_ui.char)
end

function _M.OnTask()
	uimgr.OpenUI(cf_ui.task)
end

return _M
