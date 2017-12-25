local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
    self.tex_map_RawImageEx = nil
    self.btn_close = nil

    self.OnClose = nil
end

function _M:OnLoaded()
    local function OnClose()
        uimgr.CloseUI(cf_ui.map)
    end
    
    self.OnClose = OnClose

    self.tex_map_RawImageEx = goUtil.GetComponent(self.gameObject, typeof(RawImageEx), 'tex_map')
    self.tex_map_RawImageEx:SetTextureName('cangbaotu_01')
    
    self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
    self.btn_close.onClick:AddListener(OnClose)
end

function _M:OnEnable()

end

function _M:Update(dt)

end

function _M:OnDisable()

end

function _M:OnDestroy()
    if self.OnClose then
        self.btn_close.onClick:RemoveListener(self.OnClose)
    end
end

return _M
