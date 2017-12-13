local lvmgr = require('base/lv_mgr')
local uiconfig = require('configs/cf_ui')
local uimgr = require('base/ui_mgr')

local _M = {}

function _M:OnEnter(_config, _parameter)
	lvmgr.SetLoading(1, 0.2, false)
	
	mainCamera:Init()
	uimgr.SetDefaultUI(uiconfig.main)
	uimgr.OpenCommonUI(uiconfig.input)
	
	local function callback(assetEntity)
		assetEntity:GetInstantiate()
		local _npcs = _config.npcs
		for i = 1, #_npcs, 1 do
			local _cf = _npcs[i]
			local _npc = npc.new()
			_npc:SetUID(_cf[1])
			_npc:SetPos(_cf[2], _cf[3], _cf[4])
			_npc:SetDir(_cf[5])
			table.insert(self.npcs, _npc)
		end
		
		lvmgr.SetLoading(1, 1, true)
	end
	
	local function joy(x, y, s)
		debug.LogFormat(0, tostring(self.npcs))
		local _npc = self.npcs[1]
		if _npc then
			debug.LogFormat(0, '%f, %f, %f', x, y, s)
			_tmpVec3.x = x
			_tmpVec3.y = 0
			_tmpVec3.z = y
			local _dir = goUtil.LocalToWorld(mainCamera:GetGameObject(), _tmpVec3)
			_dir.y = 0
			_npc:MoveBy(_dir, s)
		end
	end
	input.AddJoystick(joy)
	
	--mainCamera.SetPosition(Vector3(0, 15, 0))
	--mainCamera.LookAt(Vector3.zero)
	asset.AsyncLoad(asset.EAssetType.SCENE, _config.terrain, callback)
end

return _M