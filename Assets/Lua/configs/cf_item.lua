local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
	category1 = 4,
	category2 = 5,
}

local _datas = {
	[11001] = {11001, '木', 'icon_mutou', 1, 1},
	[11002] = {11002, '铁', 'icon_tie', 1, 1},
	[11003] = {11003, '皮', 'icon_pi', 1, 1},
	[11004] = {11004, '药', 'icon_yao', 1, 1},
	
	[21001] = {21001, '剑', 'icon_jian_01', 2, 1},
	[21002] = {21002, '杖', 'icon_fazhang_01', 2, 1},
	[21003] = {21003, '戟', 'icon_ji_01', 2, 1},	
}

local _index = {
	11001, 11002, 11003, 11004,
	21001, 21002, 21003,
}

local _datasReadOnly = common.ReadOnly(_datas)
local _indexReadOnly = common.ReadOnly(_index)

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