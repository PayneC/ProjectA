local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
    self.btn_map = nil

    self.OnMapClick = nil
end

function _M:OnLoaded()
    local function OnMapClick()
        uimgr.OpenUI(cf_ui.buildShop)
    end
    self.OnMapClick = OnMapClick
    
    self.btn_map = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiaju')
    self.btn_map.onClick:AddListener(self.OnMapClick)
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
