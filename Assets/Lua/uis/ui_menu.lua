local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
	self.btn_jiaju = nil
	self.btn_jiagong = nil
end

function _M:OnLoaded()
	self.btn_jiaju = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiaju')
	self.btn_jiaju.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaJu, self))
	
	self.btn_jiagong = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiagong')
	self.btn_jiagong.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaGong, self))
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
end

function _M:OnJiaJu()
	uimgr.OpenUI(cf_ui.build)
end

function _M:OnJiaGong()
	uimgr.OpenUI(cf_ui.workbench)
end

return _M
