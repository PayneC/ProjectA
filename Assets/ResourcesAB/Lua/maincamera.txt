local Vector3 = UnityEngine.Vector3
local goUtil = require('base/goutil')
local Mathf = UnityEngine.Mathf

local _M = {}

local CameraMoveType = {
	none = 0,
	drag = 1,
	aim = 2,
}

local moveType = CameraMoveType.none
local moveAim = Vector3.zero

--视觉目标点位置
local lookPosition = Vector3(- 6, - 1, - 0.5)--Vector3.zero
local moveVector = Vector3.zero
--视角距离
local distance = 0 --当前距离
--下次距离增量
local distanceAdd = 0
--屏幕对角线
local diagonal = 0
--偏航角度
local yaw = 0 --当前偏航值
local yawValue = 0 --偏航值

--俯仰角
local pitch = 0

local cameraParameter = {
	-- 移动减速曲线
	moveLerp = 0.2,
	-- 移动边界
	boundMaxX = 60,
	boundMinX = - 30,
	boundMaxZ = 18,
	boundMinZ = - 28,
	-- 距离限制
	minDistance = 3, --50,
	maxDistance = 6,
	-- 弹性值
	elasticDistance = 2,
	-- 距离移动系数
	speedDistance = 130,
	-- 俯仰角限制
	minPitch = 25, --40
	maxPitch = 45,
	-- 偏航角限制
	minYaw = 325,
	maxYaw = 325,
	-- 固定偏航角
	elasticYaw = 0,
	-- 回弹速度
	yawSpringbackSpeed = 20,
	-- 锁定偏航不自动回弹
	isLockYaw = false,
	
	-- 预设值
	defaultDistance = 85,
	defaultYaw = 325,
}

--相机
local camera
local gameobject

--临时参数
local _tempV1 = Vector3.zero
local _tempV2 = Vector3.zero

local dirty = false

local cullingMask = false

-- battle 相关
local battle_ctrl = false
local battle_target = false
local battle_mode = false
local target_pos = Vector3.zero

local function _CalculatePitch()
	local validDistance = Mathf.Clamp(distance, cameraParameter.minDistance, cameraParameter.maxDistance)
	local rangeDis =(cameraParameter.maxDistance - cameraParameter.minDistance) * 0.3
	local rangePitch = cameraParameter.maxPitch - cameraParameter.minPitch
	pitch = cameraParameter.minPitch + Mathf.Min(1,(validDistance - cameraParameter.minDistance) / rangeDis) * rangePitch
end

local function _ReCalculateAngle(angle)
	local a, b = math.modf(angle / 360)
	return angle - a * 360
end

function _M:Init()
	gameobject = goUtil.Find('Main Camera')
	camera = goUtil.GetComponent(gameobject, typeof(UnityEngine.Camera), nil)
	
	yaw = cameraParameter.defaultYaw
	distance = cameraParameter.defaultDistance
	
	_CalculatePitch()
	
	--diagonal = Mathf.Sqrt(g_width * g_width + g_height * g_height)
	goUtil.SetEulerAngles(gameobject, Vector3(pitch, yaw, 0))
	
	local pos = lookPosition - goUtil.GetForward(gameobject) * distance
	
	goUtil.SetPosition(gameobject, pos)
	
	cullingMask = camera.cullingMask
end

--- <summary>
--- 设置相机看点
--- </summary>
--- <param name="x"></param>
--- <param name="y"></param>
--- <param name="z"></param>
function _M:SetLookPosition(_x, _y, _z)
	local x = Mathf.Clamp(_x, cameraParameter.boundMinX, cameraParameter.boundMaxX)
	local z = Mathf.Clamp(_z, cameraParameter.boundMinZ, cameraParameter.boundMaxZ)
	moveAim:Set(x, _y, z)
	moveType = CameraMoveType.aim
end

--- <summary>
--- 单手指控制屏幕移动
--- </summary>
--- <param name="var1"></param>
--- <param name="var2"></param>
function _M:ScreenMoved(old_x, old_y, new_x, new_y)
	local layerMask = LayersMask.getLayerMask(LayersInt.Terrain)
	local luaRaycast = goUtil.RaycastScreenPoint(camera, old_x, old_y, layerMask)
	
	if luaRaycast.result then
		_tempV1:Set(luaRaycast.pos_x, luaRaycast.pos_y, luaRaycast.pos_z)
		
		luaRaycast = goUtil.RaycastScreenPoint(camera, new_x, new_y, layerMask)
		
		if luaRaycast.result then
			_tempV2:Set(luaRaycast.pos_x, luaRaycast.pos_y, luaRaycast.pos_z)
			moveVector = _tempV1 - _tempV2
			moveType = CameraMoveType.drag
		end
	end
end

--- <summary>
--- 两指屏幕滑动相关
--- </summary>
--- <param name="new_p1"></param>
--- <param name="new_p2"></param>
--- <param name="old_p1"></param>
--- <param name="old_p2"></param>
function _M:Screen2PointMoved(p1x, p1y, p2x, p2y, oldp1x, oldp1y, oldp2x, oldp2y)
	_tempV1:Set(oldp2x - oldp1x, oldp2y - oldp1y, 0)
	_tempV2:Set(p2x - p1x, p2y - p1y, 0)
	
	local _yaw_cross = Vector3.Dot(Vector3.forward, Vector3.Cross(_tempV1, _tempV2))
	local dir = 1
	if _yaw_cross < 0 then
		dir = - 1
	end
	
	self:DoYawBy(dir * Vector3.Angle(_tempV1, _tempV2))
	
	local old_dis = _tempV1.magnitude
	local new_dis = _tempV2.magnitude
	
	local _dif = new_dis - old_dis
	local _ScaleDif = _dif / diagonal * - 1
	self:DoDistanceBy(_ScaleDif * cameraParameter.speedDistance)
end

function _M:SetLockYaw(v)
	cameraParameter.isLockYaw = v
end

function _M:Update(dt)
	if battle_ctrl and battle_mode and battle_target then
		local v3 = goUtil.GetPosition(battle_target)
		if v3 ~= target_pos then
			target_pos = v3
			self:SetLookPosition(v3.x, v3.y, v3.z)
		end
		battle_ctrl = false
	end
	
	if distanceAdd ~= 0 then
		distance = distance + distanceAdd
		distance = Mathf.Clamp(distance, cameraParameter.minDistance - cameraParameter.elasticDistance, cameraParameter.maxDistance + cameraParameter.elasticDistance)
		_CalculatePitch()
		dirty = true
		distanceAdd = 0
	else
		if distance > cameraParameter.maxDistance then
			distance = Mathf.Lerp(distance, cameraParameter.maxDistance, cameraParameter.moveLerp)
			_CalculatePitch()
			dirty = true
		elseif distance < cameraParameter.minDistance then
			distance = Mathf.Lerp(distance, cameraParameter.minDistance, cameraParameter.moveLerp)
			_CalculatePitch()
			dirty = true
		end
	end
	
	if yawValue ~= 0 then
		yaw = yawValue + yaw
		yaw = Mathf.Clamp(yaw, cameraParameter.minYaw - cameraParameter.elasticYaw, cameraParameter.maxYaw + cameraParameter.elasticYaw)
		yaw = _ReCalculateAngle(yaw)
		dirty = true
		yawValue = 0
	elseif not cameraParameter.isLockYaw then
		if yaw > cameraParameter.maxYaw then
			yaw = Mathf.Lerp(yaw, cameraParameter.maxYaw, cameraParameter.moveLerp)
			dirty = true
		elseif yaw < cameraParameter.minYaw then
			yaw = Mathf.Lerp(yaw, cameraParameter.minYaw, cameraParameter.moveLerp)
			dirty = true
		end
	end
	
	if moveType == CameraMoveType.drag then
		local mv = lookPosition + moveVector
		local x = Mathf.Clamp(mv.x, cameraParameter.boundMinX, cameraParameter.boundMaxX)
		local z = Mathf.Clamp(mv.z, cameraParameter.boundMinZ, cameraParameter.boundMaxZ)
		lookPosition:Set(x, mv.y, z)
		
		moveVector = Vector3.Lerp(moveVector, Vector3.zero, cameraParameter.moveLerp)
		if moveVector:Equals(Vector3.zero) then
			moveType = CameraMoveType.none
		end
		
		dirty = true
		
	elseif moveType == CameraMoveType.aim then
		lookPosition = Vector3.Lerp(lookPosition, moveAim, cameraParameter.moveLerp)
		if lookPosition:Equals(moveAim) then
			moveType = CameraMoveType.none
		end
		dirty = true
	end
	
	if dirty then
		goUtil.SetEulerAngles(gameobject, Vector3(pitch, yaw, 0))
		
		local pos = lookPosition - goUtil.GetForward(gameobject) * distance
		goUtil.SetPosition(gameobject, pos)
		
		dirty = false
	end
end

function _M:DoYawBy(v)
	yawValue = v
end

function _M:DoDistanceBy(v)
	distanceAdd = v
end

function _M:GetCamera()
	return camera
end

function _M:GetGameObject()
	return gameobject
end

function _M:SetActive(_active)
	if _active then
		camera.cullingMask = cullingMask
	else
		camera.cullingMask = 0
	end
end

function _M:GetLookPosition()
	return lookPosition
end

function _M:GetMoveLerp()
	return cameraParameter.moveLerp
end

function _M:SetMoveLerp(_lerp)
	cameraParameter.moveLerp = _lerp
end

function _M:GetDistanceSpeed()
	return cameraParameter.speedDistance
end

function _M:SetDistanceSpeed(_speed)
	cameraParameter.speedDistance = _speed
end

function _M:GetCameraParameter()
	return cameraParameter
end

function _M:CtrlTarget(c)
	battle_ctrl = c
end

function _M:SetTargetCameraParameter(_minDis, _maxDis, _minPitch, _maxPitch)
	cameraParameter.minDistance = _minDis
	cameraParameter.maxDistance = _maxDis
	
	cameraParameter.minPitch = _minPitch
	cameraParameter.maxPitch = _maxPitch
end

function _M:AddTarget(gameobj)
	battle_target = gameobj
	battle_mode = true
	battle_ctrl = true
end

function _M:ScanTarget(_dis, _pitch)
	distance = _dis
	pitch = _pitch
	dirty = true
end

function _M:RemoveTarget()
	battle_target = false
	battle_mode = false
	battle_ctrl = false
	distance = cameraParameter.defaultDistance
	
	cameraParameter.minPitch = 40
	cameraParameter.maxPitch = 45
	
	pitch = cameraParameter.maxPitch
	
	cameraParameter.minDistance = 50
	cameraParameter.maxDistance = 100
	
	target_pos:Set(0, 0, 0)
	
	dirty = true
end

function _M:GetDistance()
	return distance
end

function _M:GetMinDistance()
	return cameraParameter.minDistance
end

function _M:GetMaxDistance()
	return cameraParameter.maxDistance
end

return _M
