local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
	self.btn_head_ButtonEx = nil
end

function _M:OnLoaded()
	self.btn_head_ButtonEx = nil
	
	self.btn_head_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_head')
	
	self.btn_head_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnHead, self))
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnHead()
	uimgr.OpenSubUI(cf_ui.bag)
end

return _M
