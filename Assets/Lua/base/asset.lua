local assetUtil = CS.AssetUtil.I

local _M = {}

_M.EAssetType = CS.EAssetType

function _M.AsyncLoad(type, path, callback)
    return assetUtil:AsyncLoad(type, path, callback)
end

function _M.RemoveAsyncCallback(type, path, callback)
    assetUtil:RemoveAsyncCallback(type, path, callback)
end

return _M
