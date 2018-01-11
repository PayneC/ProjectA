local cf_ui = require('configs/cf_ui')

local lvmgr = require('base/lv_mgr')
local uimgr = require('base/ui_mgr')

local _M = class()

function _M:OnEnter(levelID, parameter)
	uimgr.SetDefaultUI(cf_ui.login)
	lvmgr.SetLoading(1, 1, true)	
end

function _M:OnExit()
	uimgr.CloseUI(cf_ui.login)
end

return _M