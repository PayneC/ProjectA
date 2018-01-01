local m_workbench = require('models/m_workbench')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')

local _M = {}

-- @UID:控制台ID
-- @DID：配方ID
function _M.MakeFormula(workbenchID, formulaID)
	debug.LogFormat(0, 'MakeFormula(%d, %d) time_mgr.GetTime()=%f', workbenchID, formulaID, time_mgr.GetTime())
	m_workbench.SetWorkbench(workbenchID, formulaID, time_mgr.GetTime())
end

return _M 