local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local uiconfig = require('configs/cf_ui')
local goUtil = require('base/goutil')

local _M = class(uibase)

function _M:ctor()
    self.txt_info = false
end

function _M:OnLoaded()
    self.txt_info = goUtil.GetComponent(self.gameObject, typeof(CS.TextEx), 'txt_info')

    self.txt_info.text = 'payne'
end

function _M:OnEnable()

end

function _M:Update(dt)

end

function _M:OnDisable()

end

function _M:OnDestroy()

end

return _M