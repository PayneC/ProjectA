local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local uiconfig = require('configs/cf_ui')
local goUtil = require('base/goutil')
local lvmgr = require('base/lv_mgr')
local cf_lv = require('configs/cf_lv')

local _M = class(uibase)

function _M:ctor()
    self.btn_start = nil
end

function _M:OnLoaded()
    local function OnGameStart()
        lvmgr.LoadLevel(cf_lv.GetDataByID(1))
    end
    self.OnGameStart = OnGameStart
    
    self.btn_start = goUtil.GetComponent(self.gameObject, typeof(CS.ButtonEx), 'btn_start')
    self.btn_start.onClick:AddListener(self.OnGameStart)
end

function _M:OnEnable()

end

function _M:Update(dt)

end

function _M:OnDisable()

end

function _M:OnDestroy()
    if self.OnMapClick then
        self.btn_map.onClick:RemoveListener(self.OnMapClick)
    end
end

return _M