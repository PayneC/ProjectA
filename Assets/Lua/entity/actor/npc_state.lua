local define = require('commons/define')
local ActorState = define.ActorState
local goUtil = require('base/goutil')
local CollisionFlags = UnityEngine.CollisionFlags

local idle = {}
function idle.Enter(self)
	if self.animation then
		self.animation:Play('Idle')
	end
end

function idle.Check(self, dt)
	if self.moveDir.x ~= 0 or self.moveDir.z ~= 0 then
		return ActorState.RUN
	end
	return false
end

function idle.Exit(self)
	
end

local run = {}
function run.Enter(self)
	if self.animation then
		self.animation:Play('Walk')
	end
end

local tempY = 0
function run.Check(self, dt)
	if self.moveDir.x ~= 0 or self.moveDir.z ~= 0 then
		if self.gameObject then
			local collisionFlag = self.characterController:Move(self.moveDir * dt * 3)
			
			tempY = self.moveDir.y
			self.moveDir.y = 0
			goUtil.SetForward(self.gameObject, self.moveDir)			
			self.moveDir.y = tempY
			
			if not self.characterController.isGrounded then
				self.moveDir.y = - 10
			else
				self.moveDir.y = 0
			end
			
			self.pos = goUtil.GetPosition(self.gameObject)
		end
	else
		return ActorState.IDLE
	end
	return false
end

function run.Exit(self)
	
end

local skill = {}
function skill.Enter(self)
	
end

function skill.Check(self, dt)
	return false
end

function skill.Exit(self)
	
end

local _M = {
	[ActorState.IDLE] = idle,
	[ActorState.RUN] = run,
	[ActorState.Skill] = skill,
}

return _M
