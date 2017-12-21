local lvmgr = require('base/lv_mgr')
local uiconfig = require('configs/cf_ui')
local uimgr = require('base/ui_mgr')

local _M = class()

function _M:OnEnter(_config, _parameter)
	lvmgr.SetLoading(1, 0.2, true)	
	uimgr.SetDefaultUI(uiconfig.login)
end

function _M:OnExit()
	uimgr.CloseUI(uiconfig.login)
end

return _M