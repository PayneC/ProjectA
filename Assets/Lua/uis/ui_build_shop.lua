local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local uiconfig = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

local function NewItem(_ui)
    local gameObject= goUtil.Instantiate(_ui.rt_item)
    local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(CS.ImageEx), 'spr_icon')
    local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(CS.TextEx), 'txt_name')
    local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(CS.TextEx), 'txt_num')
    
    local spr_item_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(CS.ImageEx), 'spr_item_icon')
    local txt_item_name_TextEx = goUtil.GetComponent(gameObject, typeof(CS.TextEx), 'txt_item_name')
    local txt_item_num_TextEx = goUtil.GetComponent(gameObject, typeof(CS.TextEx), 'txt_item_num')
    
    local _item = {
        DID = 0,
        gameObject = gameObject,
        spr_icon_ImageEx = spr_icon_ImageEx,
        txt_name_TextEx = txt_name_TextEx,
        txt_num_TextEx = txt_num_TextEx,
        spr_item_icon_ImageEx = spr_item_icon_ImageEx,
        txt_item_name_TextEx = txt_item_name_TextEx,
        txt_item_num_TextEx = txt_item_num_TextEx,
    }

    function _item:SetData(DID)
        self.DID = DID
            
    end

    return _item
end

function _M:ctor()
    self.btn_close = nil
    self.rt_item = nil

    self.OnClose = nil
end

function _M:OnLoaded()
    local function OnClose()
        uimgr.CloseUI(uiconfig.buildShop)
    end
    
    self.OnClose = OnClose
    
    self.btn_close = goUtil.GetComponent(self.gameObject, typeof(CS.ButtonEx), 'btn_close')
    self.btn_close.onClick:AddListener(OnClose)

    self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
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