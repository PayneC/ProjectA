-- Description:
-- 除标记为Common 的 UI以外其他UI只能同时打开一个主要UI
-- 可打开多个附加UI
-- 附加UI会在主UI关闭同时被关闭


local UIActiveType =
    {
        none = 0,
        main = 1,
        sub = 2,
        common = 3,
    }

local _M = {}

-- 缺省UI
local defaultUI = false
-- 当前显示的UI
local mainUI = {config = false, ui = false}
-- 当前UI的附加UI
local subUIs = {}
-- 通用UI
local commonUIS = {}

-- 已经加载过的UI
local allUIs = {}

local function _CloseAllSubUIs()
    for i = 1, #subUIs, 1 do
        _ui = subUIs[i]
        if _ui then
            _ui.ui:SetActive(false)
        end
    end
    subUIs = {}
end

function _M.Update(dt)
    local _ui = mainUI.ui
    
    if _ui and _ui:IsLoaded() and _ui.Update then
        _ui:Update(dt)
    end
    
    for i = 1, #subUIs, 1 do
        _ui = subUIs[i]
        _ui = _ui and _ui.ui
        if _ui and _ui:IsLoaded() and _ui.Update then
            _ui:Update(dt)
        end
    end
    
    for i = 1, #commonUIS, 1 do
        _ui = commonUIS[i]
        _ui = _ui and _ui.ui
        if _ui and _ui:IsLoaded() and _ui.Update then
            _ui:Update(dt)
        end
    end
end

-- 以主UI的类型打开
-- @_config : UI信息
-- @_parameter : 参数
function _M.OpenUI(_config, _parameter)
    if not _config then
        debug.LogError('ui_mgr OpenUI _config is nil')
        return nil
    end
    
    local _openType = _M.CheckUIOpenType(_config)
    
    if _openType ~= UIActiveType.none and _openType ~= UIActiveType.main then
        debug.LogFormat('ui_mgr', "OpenUI 该UI = %d 已经以类型%d打开", _config.id, _openType)
        return nil
    end
    
    local _ui = _M.GetUI(_config)
    if _openType == UIActiveType.main then
        return _ui
    end
    
    _CloseAllSubUIs()
    
    if mainUI.ui then
        mainUI.ui:SetActive(false)
        mainUI.config = false
        mainUI.ui = false
    end
    
    if _ui then
        mainUI.config = _config
        mainUI.ui = _ui
        
        _ui:SetActive(true, _parameter)
    end
    
    return _ui
end

-- 以子UI的类型打开
-- @_config : UI信息
-- @_parameter : 参数
function _M.OpenSubUI(_config, _parameter)
    if not _config then
        debug.LogError('ui_mgr OpenSubUI _config is nil')
        return nil
    end
    
    local _openType = _M.CheckUIOpenType(_config)
    
    if _openType ~= UIActiveType.none and _openType ~= UIActiveType.sub then
        debug.LogFormat('ui_mgr', "OpenSubUI 该UI = %d 已经以类型%d打开", _config.id, _openType)
        return nil
    end
    
    local _ui = _M.GetUI(_config)
    
    if _openType == UIActiveType.sub then
        return _ui
    end
    
    if _ui then
        table.insert(subUIs, {config = _config, ui = _ui})
        _ui:SetActive(true, _parameter)
    end
    
    return _ui
end

-- 以通用UI的类型打开
-- @_config : UI信息
-- @_parameter : 参数
function _M.OpenCommonUI(_config, _parameter)
    if not _config then
        debug.LogError('ui_mgr OpenCommonUI _config is nil')
        return nil
    end
    
    local _openType = _M.CheckUIOpenType(_config)
    
    if _openType ~= UIActiveType.none and _openType ~= UIActiveType.common then
        debug.LogFormat('ui_mgr', "OpenCommonUI 该UI = %d 已经以类型%d打开", _config.id, _openType)
        return nil
    end
    
    local _ui = _M.GetUI(_config)
    
    if _openType == UIActiveType.common then
        return _ui
    end
    
    if _ui then
        table.insert(commonUIS, {config = _config, ui = _ui})
        _ui:SetActive(true, _parameter)
    end
    
    return _ui
end

-- 设置缺省UI
-- @_config : UI信息
-- @_parameter : 参数
function _M.SetDefaultUI(_config)
    if not _config then
        debug.LogError('ui_mgr SetDefaultUI _config is nil')
        return nil
    end
    
    local _openType = _M.CheckUIOpenType(_config)
    
    if _openType ~= UIActiveType.none and _openType ~= UIActiveType.main then
        debug.LogFormat('ui_mgr', "SetDefaultUI 该UI = %d 已经以类型%d打开", _config.id, _openType)
        return nil
    end
    
    defaultUI = _config
    
    if not mainUI.config then
        _M.OpenUI(defaultUI)
    end
end

function _M.CloseUI(_config)
    if not _config then
        debug.LogError('ui_mgr CloseUI _config is nil')
        return nil
    end
    
    local _openType = _M.CheckUIOpenType(_config)
    
    if _openType == UIActiveType.main then
        _CloseAllSubUIs()
        mainUI.config = false
        mainUI.ui = false
    elseif _openType == UIActiveType.sub then
        for i = 1, #subUIs, 1 do
            if subUIs[i].config == _config then
                table.remove(subUIs, i)
                break
            end
        end
    elseif _openType == UIActiveType.common then
        for i = 1, #commonUIS, 1 do
            if commonUIS[i].config == _config then
                table.remove(commonUIS, i)
                break
            end
        end
    end
    
    if _openType ~= UIActiveType.none then
        local _ui = _M.GetUI(_config)
        if _ui then
            _ui:SetActive(false)
        end
    end
    
    if _config == defaultUI then
        defaultUI = false
    elseif _openType == UIActiveType.main and defaultUI then
        _M.OpenUI(defaultUI)
    end
end

function _M.GetUI(_config)
    if not _config then
        debug.LogError('ui_mgr GetUI _config is nil')
        return nil
    end
    
    local _ui = allUIs[_config]
    
    if not _ui then
        local _script = require(_config.script)
        
        if _script then
            local _index = 0
            local _depth = 0
            for k, v in pairs(allUIs) do
                _depth = (v and v.depth) or 0
                if _config.depth >= _depth then
                    _index = _index + 1
                end
            end
            
            _ui = _script.new(_config, _index)
            allUIs[_config] = _ui
        else
            debug.LogErrorFormat('ui_mgr GetUI not find ui script id = %d', _config.id)
        end
    end
    
    if not _ui then
        debug.LogErrorFormat('ui_mgr GetUI not find ui id = %d', _config.id)
    end
    
    return _ui
end

function _M.CheckUIOpenType(_config)
    if _config == mainUI.config then
        return UIActiveType.main
    else
        for i = 1, #subUIs, 1 do
            if _config == subUIs[i].config then
                return UIActiveType.sub
            end
        end
        
        for i = 1, #commonUIS, 1 do
            if _config == commonUIS[i].config then
                return UIActiveType.common
            end
        end
    end
    return UIActiveType.none
end

function _M.OnDestroy()
    for _config, _ui in pairs(allUIs) do
        if _ui then
            if _ui:IsLoaded() then
                _ui:SetActive(false)
                if _ui.OnDestroy then
                    _ui:OnDestroy()
                end
            else
                _ui:UnLoad()
            end
        end
    end
end

local mt = {
    __newindex = function(t, k, v)
        debug.LogError("attempt to update a read-only table")
    end
}
setmetatable(_M, mt)

return _M
