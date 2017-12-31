local cf_lv = require('configs/cf_lv')
local cf_ui = require('configs/cf_ui')

local lvmgr = require('base/lv_mgr')
local uimgr = require('base/ui_mgr')

local goUtil = require('base/goutil')
local asset = require('base/asset')
local input = require('base/input')

local Vector3 = UnityEngine.Vector3

local _M = class()

function _M:ctor()
	self.scene = false
end

function _M:OnEnter(levelID, parameter)
	lvmgr.SetLoading(1, 0.2, true)

	uimgr.OpenCommonUI(cf_ui.main)
	uimgr.SetDefaultUI(cf_ui.workbench)
	
	lvmgr.SetLoading(1, 1, true)
end

function _M:OnExit()
	uimgr.CloseUI(cf_ui.login)
end

return _M 