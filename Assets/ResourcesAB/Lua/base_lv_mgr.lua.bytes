-- Description:
-- 主关卡只能同时存在一个
-- 切换关卡时会自动打开Loading界面需要在新关卡进入后合适时机手动关闭
local cf_lv = require('configs/cf_lv')

local goUtil = GameObjectUtil --require('base/goutil')


local _M = {}

local _uiLoading = false

local _currentLevel = false
local _currentLevelID = false

local _nextLevelID = false
local _nextLevelNeedLoading = false
local _nextLevelParameter = false

local function _LoadNextLevel()
	local _scriptName = cf_lv.GetData(_nextLevelID, cf_lv.script)
	local _loadingBG = cf_lv.GetData(_nextLevelID, cf_lv.loadingBG)
	
	local _script = require(_scriptName)
	if _script then
		--[[        if _loadingBG then
			_uiLoading:SetLoadingBg(_loadingBG)
		else
			_uiLoading:SetLoadingBg(nil)
        end
        ]]
		if _nextLevelNeedLoading then
			_uiLoading:OpenLoading()
		end
		
		_M.UnloadLevel()
		_currentLevelID = _nextLevelID
		_currentLevel = _script.new()
		if _currentLevel and _currentLevel.OnEnter then
			_currentLevel:OnEnter(_currentLevelID, _nextLevelParameter)
		end
	else
		log.error('scene_mgr LoadScene error:', 'require _script.new nil!')
	end
	
	_nextLevelID = false
	_nextLevelNeedLoading = false
	_nextLevelParameter = false
end

function _M.Init()
	local go = goUtil.Find('UILoading')
	_uiLoading = goUtil.GetComponent(go, typeof(UILoading), nil)
end

function _M.Update(dt)
	if _nextLevelID then
		_LoadNextLevel()
	end
	
	if _currentLevel and _currentLevel.Update then
		_currentLevel:Update(dt)
	end
end

function _M.LoadLevel(levelID, parameter, needLoading)
	if levelID and levelID ~= _currentLevelID then
		_nextLevelID = levelID or false
		_nextLevelParameter = parameter or false
		_nextLevelNeedLoading = needLoading or false
	end
end

function _M.ReloadLevel(levelID, parameter, needLoading)
	if levelID then
		_nextLevelID = levelID or false
		_nextLevelParameter = parameter or false
		_nextLevelNeedLoading = needLoading or false
	end
end

function _M.UnloadLevel()
	if _currentLevel and _currentLevel.OnExit then
		_currentLevel:OnExit()
	end
	_currentLevel = false
	_currentLevelID = false
end

function _M.GetLevel()
	return _currentLevel
end

function _M.GetLevelID()
	return _currentLevelID
end

function _M.SetLoading(t, p, autoClose)
	_uiLoading:SetLoading(t or 0, p or 0, autoClose or false)
end

return _M
