local debug = require('base/debug')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
	LV = 4,
	unlockCondition = 5,
	unlockCost = 6,
	nextLV = 7,
	itemCapacity = 8,
	itemID = 9,
	speed = 10,
	itemStorage = 11,
}

local _datas = {
	[40000101] = {40000101, '木桶', 'build_mutong', 1, false, {200, 300}, 40000102, 25, 10100101, 60, 60},
	[40000102] = {40000102, '木桶', 'build_mutong', 2, false, {200, 300}, 40000103, 50, 10100101, 90, 90},
	[40000103] = {40000103, '木桶', 'build_mutong', 3, false, {200, 300}, 40000104, 75, 10100101, 120, 120},
	[40000104] = {40000104, '木桶', 'build_mutong', 4, false, {200, 300}, 40000105, 100, 10100101, 150, 150},
	[40000105] = {40000105, '木桶', 'build_mutong', 5, false, false, false, 125, 10100101, 180, 180},
	
	[40000201] = {40000201, '铁桶', 'build_tietong', 1, false, {200, 300}, 40000202, 25, 10100102, 60, 60},
	[40000202] = {40000202, '铁桶', 'build_tietong', 2, false, {200, 300}, 40000203, 50, 10100102, 90, 90},
	[40000203] = {40000203, '铁桶', 'build_tietong', 3, false, {200, 300}, 40000204, 75, 10100102, 120, 120},
	[40000204] = {40000204, '铁桶', 'build_tietong', 4, false, {200, 300}, 40000205, 100, 10100102, 150, 150},
	[40000205] = {40000205, '铁桶', 'build_tietong', 5, false, false, false, 125, 10100102, 180, 180},
	
	[40000301] = {40000301, '皮桶', 'build_pitong', 1, false, {200, 300}, 40000302, 25, 10100103, 60, 60},
	[40000302] = {40000302, '皮桶', 'build_pitong', 2, false, {200, 300}, 40000303, 50, 10100103, 90, 90},
	[40000303] = {40000303, '皮桶', 'build_pitong', 3, false, {200, 300}, 40000304, 75, 10100103, 120, 120},
	[40000304] = {40000304, '皮桶', 'build_pitong', 4, false, {200, 300}, 40000305, 100, 10100103, 150, 150},
	[40000305] = {40000305, '皮桶', 'build_pitong', 5, false, false, false, 125, 10100103, 180, 180},
	
	[40000401] = {40000401, '药桶', 'build_yaotong', 1, false, {200, 300}, 40000402, 25, 10100104, 60, 60},
	[40000402] = {40000402, '药桶', 'build_yaotong', 2, false, {200, 300}, 40000403, 50, 10100104, 90, 90},
	[40000403] = {40000403, '药桶', 'build_yaotong', 3, false, {200, 300}, 40000404, 75, 10100104, 120, 120},
	[40000404] = {40000404, '药桶', 'build_yaotong', 4, false, {200, 300}, 40000405, 100, 10100104, 150, 150},
	[40000405] = {40000405, '药桶', 'build_yaotong', 5, false, false, false, 125, 10100104, 180, 180},
}

local _index = {
	40000101, 40000102, 40000103, 40000104, 40000105,
	40000201, 40000202, 40000203, 40000204, 40000205,
	40000301, 40000302, 40000303, 40000304, 40000305,
	40000401, 40000402, 40000403, 40000404, 40000405,
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
	debug.LogErrorFormat('cf_bulid not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M 