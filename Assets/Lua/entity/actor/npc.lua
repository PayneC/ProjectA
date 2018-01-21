local Vector3 = UnityEngine.Vector3
local csv_npc = require('csv/csv_npc')
local asset = require('base/asset')
local goUtil = require('base/goutil')
local npc_state = require('entity/actor/npc_state')
-- User: ct
-- Date: 2017-10-31 14:56
-- Description: AIController
local _M = class()

function _M:ctor()
	-- 数据
	self.UID = 0 --动态ID
	self.data = false --动态ID关联的数据引用 数据来自数据管理类
	
	--组件
	self.gameObject = false
	self.characterController = false
	self.navMeshAgent = false
	self.animation = false
	
	self.controller = false
	
	-- TEMP 以下数据均存在于data下
	self.pos = Vector3.zero
	self.dir = Vector3.zero
	
	self.moveDir = Vector3.zero
	
	self.stateID = false
	self.nextStateID = 1
	
	self.state = false
	self.stateMap = npc_state
end

function _M:GetStateID()
	return self.stateID
end

function _M:SetUID(_uid)	
	-- 设置UID并获取配置数据
	local _asset = csv_npc.GetData(_uid, csv_npc.asset)
	
	local function callback(assetEntity)
		self.gameObject = assetEntity:GetInstantiate()
		goUtil.SetPosition(self.gameObject, self.pos)
		goUtil.SetEulerAngles(self.gameObject, self.dir)
		
		self.animation = goUtil.GetComponent(self.gameObject, typeof(UnityEngine.Animation), nil)
		self.characterController = goUtil.GetComponent(self.gameObject, typeof(UnityEngine.CharacterController), nil)
	end
	
	asset.AsyncLoad(asset.EAssetType.ENTITY, _asset, callback)
end

function _M:SetPos(_x, _y, _z)
	self.pos.x = _x
	self.pos.y = _y
	self.pos.z = _z
	if self.gameObject then
		--goUtil.SetPosition(self.gameObject, self.pos)
		self.characterController:Move(self.pos)
	end
end

function _M:SetDir(_dir)
	self.dir.x = 0
	self.dir.y = _dir
	self.dir.z = 0
	if self.gameObject then
		goUtil.SetEulerAngles(self.gameObject, self.dir)
	end
end

function _M:MoveBy(_dir, _s)
	self.moveDir.x = _dir.x
	--self.moveDir.y = _dir.y
	self.moveDir.z = _dir.z
end

function _M:Update(dt)
	if self.nextStateID then
		if self.state and self.state.Exit then
			self.state.Exit(self)
		end
		
		self.stateID = self.nextStateID
		self.state = self.stateMap[self.stateID]
		
		if self.state and self.state.Enter then
			self.state.Enter(self)
		end
		
		self.nextState = false
	end
	
	if self.state and self.state.Check then
		self.nextStateID = self.state.Check(self, dt)
	end
end

function _M:OnDestroy()
	self.state = false
	self.nextState = false
end

function _M:PlayAnimation(animationName)
	
end

function _M:PlayEffect(effectName)
	
end

return _M
