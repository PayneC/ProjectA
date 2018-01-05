local _M = {}
_M.ActorState = {
	IDLE = 1,
	RUN = 2,
	Skill = 3,
}

_M.event = {
	BuildChange = 1,
	BuildLVChange = 2,
	WorkbenchChange = 3,
	ItemChange = 4,
	StuffChange = 5,
}
return _M
