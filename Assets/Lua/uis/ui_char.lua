local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
	self.btn_close = nil
end

function _M:OnLoaded()
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.btn_close.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:Close()
    uimgr.CloseUI(cf_ui.char)
end

return _M 