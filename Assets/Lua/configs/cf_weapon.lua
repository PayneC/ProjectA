local debug = require('base/debug')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
}

local _datas = {
	[20100101] = {20100101, '剑1', 'item_246-1'},
	[20100201] = {20100201, '剑2', 'item_247-1'},
	[20100301] = {20100301, '剑3', 'item_248-1'},
	[20100401] = {20100401, '剑4', 'item_249-1'},
	[20100501] = {20100501, '剑5', 'item_251-1'},
	[20100601] = {20100601, '剑6', 'item_252-1'},
	[20100701] = {20100701, '剑7', 'item_253-1'},
	[20100801] = {20100801, '剑8', 'item_254-1'},
	
	[20200101] = {20200101, '棍1', 'item_440-1'},
	[20200201] = {20200201, '棍2', 'item_441-1'},
	[20200301] = {20200301, '棍3', 'item_442-1'},
	[20200401] = {20200401, '棍4', 'item_443-1'},
	[20200501] = {20200501, '棍5', 'item_444-1'},
	[20200601] = {20200601, '棍6', 'item_445-1'},
	
	[20300101] = {20300101, '扇子1', 'item_378-1'},
	[20300201] = {20300201, '扇子2', 'item_379-1'},
	[20300301] = {20300301, '扇子3', 'item_380-1'},
	[20300401] = {20300401, '扇子4', 'item_381-1'},
	[20300501] = {20300501, '扇子5', 'item_382-1'},
	[20300601] = {20300601, '扇子6', 'item_383-1'},
	[20300701] = {20300701, '扇子7', 'item_384-1'},
}

local _index = {
	20100101, 20100201, 20100301, 20100401, 20100501, 20100601, 20100701, 20100801,
	20200101, 20200201, 20200301, 20200401, 20200501, 20200601,
	20300101, 20300201, 20300301, 20300401, 20300501, 20300601, 20300701,
}

local _datasReadOnly = readonly(_datas)
local _indexReadOnly = readonly(_index)

function _M.GetData(_id, _type)
	local _data = _M.GetDataEntire(_id)
	if _data then
		return _data[_type]
	end
	return nil
end

function _M.GetDataEntire(_id)
	local _data = _datas[_id]
	if _data then
		return _data
	end
	debug.LogErrorFormat('cf_weapon not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M 