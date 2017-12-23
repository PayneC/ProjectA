local cf_lv = require('configs/cf_lv')
local cf_ui = require('configs/cf_ui')

local lvmgr = require('base/lv_mgr')
local uimgr = require('base/ui_mgr')

local goUtil = require('base/goutil')
local asset = require('base/asset')
local input = require('base/input')

local mainCamera = require('entity/camera/maincamera')
local Vector3 = CS.UnityEngine.Vector3

local _M = class()

function _M:ctor()
	self.scene = false
end

function _M:OnEnter(levelID, parameter)
	lvmgr.SetLoading(1, 0.2, true)	
	
	mainCamera:Init()
	mainCamera:SetLookPosition(0, 0, 0)
	
	uimgr.OpenCommonUI(cf_ui.input)
	uimgr.OpenCommonUI(cf_ui.config)
	
	local function callback(assetEntity)
		self.scene = assetEntity:GetInstantiate()
		lvmgr.SetLoading(1, 1, true)
	end
	
	local cf_parameter = cf_lv.GetData(levelID, cf_lv.parameter)
	asset.AsyncLoad(asset.EAssetType.SCENE, cf_parameter.terrain, callback)
end

function _M:OnExit()
	uimgr.CloseUI(cf_ui.login)
end

return _M 