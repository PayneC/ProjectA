-- Description:
-- 主关卡只能同时存在一个
-- 切换关卡时会自动打开Loading界面需要在新关卡进入后合适时机手动关闭
local goUtil = require('base/goutil')
local _uiLoading = false

local _M = {}
local _currentScene = false
local _currentSceneInfo = false

function _M.Init()
    local go = goUtil.Find('UILoading')
    _uiLoading = goUtil.GetComponent(go, typeof(CS.UILoading))
end

function _M.Update(dt)
    if _currentScene and _currentScene.Update then
        _currentScene:Update(dt)
    end
end

function _M.LoadLevel(_config, _parameter, _needLoading)
    -- if _config.bg then
    --     _uiLoading:SetLoadingBg(_config.bg)
    -- else
    --     _uiLoading:SetLoadingBg(nil)
    -- end
    if _needLoading then
        _uiLoading:OpenLoading()
    end
    
    _M.UnloadLevel()
    
    _currentSceneInfo = _config
    if _currentSceneInfo then
        local _script = require(_currentSceneInfo.script)
        _currentScene = _script.new()
        if _currentScene then
            if _currentScene.OnEnter then
                _currentScene:OnEnter(_config, _parameter)
            end
        else
            log.error('scene_mgr LoadScene error:', 'require _script.new nil!')
        end
    end
    
    return _currentScene
end

function _M.UnloadLevel()
    if _currentScene and _currentScene.OnExit then
        _mainScene:OnExit()
    end
    _currentScene = false
    _currentSceneInfo = false
end

function _M.GetLevel()
    return _currentScene
end

function _M.GetSceneInfo()
    return _currentSceneInfo
end

function _M.SetLoading(t, p, autoClose)
    _uiLoading:SetLoading(t or 0, p or 0, autoClose or false)
end

return _M
