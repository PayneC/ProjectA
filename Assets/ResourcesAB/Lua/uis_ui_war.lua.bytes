local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
	self.btn_char = nil	
end

function _M:OnLoaded()
	self.btn_char = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_char')
	self.btn_char.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnChar, self))
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnChar()
    uimgr.OpenSubUI(cf_ui.char)
end

return _M 