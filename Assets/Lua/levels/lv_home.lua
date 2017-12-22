local lvmgr = require('base/lv_mgr')
local uiconfig = require('configs/cf_ui')
local uimgr = require('base/ui_mgr')

local asset = require('base/asset')

local mainCamera = require('entity/camera/maincamera')
local Vector3 = CS.UnityEngine.Vector3

local input = require('base/input')
local goUtil = require('base/goutil')

local _M = class()

function _M:ctor()
	self.scene = false
end

function _M:OnEnter(_config, _parameter)
	lvmgr.SetLoading(1, 0.2, true)	

	mainCamera:Init()
	mainCamera:SetLookPosition(0, 0, 0)
	
	uimgr.OpenCommonUI(uiconfig.input)
	uimgr.OpenCommonUI(uiconfig.config)
	
	local function callback(assetEntity)
		self.scene = assetEntity:GetInstantiate()
		lvmgr.SetLoading(1, 1, true)
	end

	local function joy(x, y, s)
		debug.LogFormat(0, tostring(self.npcs))
		local _npc = self.npcs[1]
		if _npc then
			--debug.LogFormat(0, '%f, %f, %f', x, y, s)
			_tmpVec3.x = x
			_tmpVec3.y = 0
			_tmpVec3.z = y
			local _dir = goUtil.LocalToWorld(mainCamera:GetGameObject(), _tmpVec3)
			_dir.y = 0
			_npc:MoveBy(_dir, s)
		end
	end
	input.AddJoystick(joy)
	
	asset.AsyncLoad(asset.EAssetType.SCENE, _config.terrain, callback)
end

function _M:OnExit()
	uimgr.CloseUI(uiconfig.login)
end

return _M