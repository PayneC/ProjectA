local goUtil = require('base/goutil')
local asset = require('base/asset')

local Vector2 = UnityEngine.Vector2
local Vector3 = UnityEngine.Vector3

local _uiRoot = false

local function AddUIToRoot(_gameobject, _depth)
    if not _gameobject then
        return
    end
    if not _uiRoot then
        _uiRoot = goUtil.Find("UIRoot")
    end
    if _uiRoot then
        goUtil.SetParent(_gameobject, _uiRoot)
        
        local _transform = _gameobject.transform
        _transform.offsetMin = Vector2.zero
        _transform.offsetMax = Vector2.zero
        _transform:SetSiblingIndex(_depth)
        
        goUtil.SetLocalScale(_gameobject, Vector3.one)
    end
end

local _M = class()

--构造函数
function _M:ctor(_data, _SiblingIndex)
    self.id = _data.id
    self.assetname = _data.assetsName
    self.depth = _data.depth
    self.AnimationType = _data.AnimationType
    self.SiblingIndex = _SiblingIndex
    
    self.isActive = false
    self.isLoading = false
    self.isLoaded = false
    
    self.gameObject = nil
    self.transform = nil
    
    self.callback = false
end

--打开界面
function _M:SetActive(_active, _parameter)
    if _active then
        --预制体加载完成回调
        local function callback(assetEntity)
            self.gameObject = assetEntity:GetInstantiate()
            self.transform = self.gameObject.transform
            goUtil.SetActive(self.gameObject, self.isActive)
            
            self.isLoading = false
            AddUIToRoot(self.gameObject, self.SiblingIndex)
            
            self.isLoaded = true
            if self.OnLoaded then
                self:OnLoaded(obj)
            end
            
            if self.isActive and self.OnEnable then
                if self.AnimationType then
                    self.transform:DOScale(0, 0)
                    self.transform:DOScale(1, 0.3):SetEase(Ease.OutBack)
                end
                self:OnEnable(_parameter)
            end
        end
        
        self.callback = callback
        
        if not self.isActive then
            self.isActive = true
            if not self.gameObject then
                if not self.isLoading then
                    self.isLoading = true
                    asset.AsyncLoad(asset.EAssetType.UI, self.assetname, callback)
                end
            else
                goUtil.SetActive(self.gameObject, self.isActive)
                if self.OnEnable then
                    if self.AnimationType then
                        self.transform:DOScale(0, 0)
                        self.transform:DOScale(1, 0.3):SetEase(Ease.OutBack)
                    end
                    self:OnEnable(_parameter)
                end
            end
        end
    else
        if self.isActive then
            self.isActive = false
            if self.gameObject then
                if self.OnDisable then
                    self:OnDisable()
                end
                goUtil.SetActive(self.gameObject, self.isActive)
            end
        end
    end
end

function _M:Destroy()
    goUtil.Destroy(self.gameObject)
    
    self.gameObject = nil
    self.transform = nil
    if self.OnDestroy then
        self:OnDestroy()
    end
end

function _M:IsActive()
    return self.isActive
end

function _M:IsLoaded()
    return self.isLoaded
end

function _M:UnLoad()
    if self:IsLoaded() then
        -- 资源回收
        else
        -- 取消资源加载监听
        if self.callback then
            asset.RemoveAsyncCallback(asset.EAssetType.UI, self.assetname, self.callback)
        end
    end
end

return _M
