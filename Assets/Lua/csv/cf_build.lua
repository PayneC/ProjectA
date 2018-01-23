

local _M = {
	id = 1, -- 数据ID
	name = 2, -- 名称
	icon = 3, -- 图标
	LV = 4, -- 等级
	unlockCondition = 5, -- 解锁条件
	unlockCost = 6, -- 解锁花费
	nextLV = 7, -- 下一等级
	itemCapacity = 8, -- 物品容量
	itemID = 9, -- 物品ID
	speed = 10, -- 生产速度
	itemStorage = 11, -- 物品存储增加
	
}

local _datas = {
	[30010001] = {30010001, '木材建筑', false, 1, false, {{300, 0}}, 30010002, 20, 11101001, 60, 50},
	[30010002] = {30010002, '木材建筑', false, 2, false, {{600, 0}}, 30010003, 40, 11101001, 60, 100},
	[30010003] = {30010003, '木材建筑', false, 3, false, {{1400, 0}}, 30010004, 60, 11101001, 60, 150},
	[30010004] = {30010004, '木材建筑', false, 4, false, {{3000, 0}}, 30010005, 80, 11101001, 60, 200},
	[30010005] = {30010005, '木材建筑', false, 5, false, {{6400, 0}}, 30010006, 100, 11101001, 60, 250},
	[30010006] = {30010006, '木材建筑', false, 6, false, {{13800, 0}}, 30010007, 150, 11101001, 60, 375},
	[30010007] = {30010007, '木材建筑', false, 7, false, {{29600, 0}}, 30010008, 200, 11101001, 60, 500},
	[30010008] = {30010008, '木材建筑', false, 8, false, {{63700, 0}}, 30010009, 250, 11101001, 60, 625},
	[30010009] = {30010009, '木材建筑', false, 9, false, {{137000, 0}}, 30010010, 300, 11101001, 60, 750},
	[30010010] = {30010010, '木材建筑', false, 10, false, {{294500, 0}}, false, 400, 11101001, 60, 1000},
	[30020001] = {30020001, '精铁建筑', false, 1, false, {{500, 0}}, 30020002, 20, 11101002, 40, 50},
	[30020002] = {30020002, '精铁建筑', false, 2, false, {{1100, 0}}, 30020003, 40, 11101002, 40, 100},
	[30020003] = {30020003, '精铁建筑', false, 3, false, {{2300, 0}}, 30020004, 60, 11101002, 40, 150},
	[30020004] = {30020004, '精铁建筑', false, 4, false, {{5000, 0}}, 30020005, 80, 11101002, 40, 200},
	[30020005] = {30020005, '精铁建筑', false, 5, false, {{10700, 0}}, 30020006, 100, 11101002, 40, 250},
	[30020006] = {30020006, '精铁建筑', false, 6, false, {{23000, 0}}, 30020007, 150, 11101002, 40, 375},
	[30020007] = {30020007, '精铁建筑', false, 7, false, {{49400, 0}}, 30020008, 200, 11101002, 40, 500},
	[30020008] = {30020008, '精铁建筑', false, 8, false, {{106200, 0}}, 30020009, 250, 11101002, 40, 625},
	[30020009] = {30020009, '精铁建筑', false, 9, false, {{228300, 0}}, 30020010, 300, 11101002, 40, 750},
	[30020010] = {30020010, '精铁建筑', false, 10, false, {{490800, 0}}, false, 400, 11101002, 40, 1000},
	[30030001] = {30030001, '布料建筑', false, 1, false, {{800, 0}}, 30030002, 20, 11101003, 12, 50},
	[30030002] = {30030002, '布料建筑', false, 2, false, {{1760, 0}}, 30030003, 40, 11101003, 12, 100},
	[30030003] = {30030003, '布料建筑', false, 3, false, {{3680, 0}}, 30030004, 60, 11101003, 12, 150},
	[30030004] = {30030004, '布料建筑', false, 4, false, {{8000, 0}}, 30030005, 80, 11101003, 12, 200},
	[30030005] = {30030005, '布料建筑', false, 5, false, {{17120, 0}}, 30030006, 100, 11101003, 12, 250},
	[30030006] = {30030006, '布料建筑', false, 6, false, {{36800, 0}}, 30030007, 150, 11101003, 12, 375},
	[30030007] = {30030007, '布料建筑', false, 7, false, {{79040, 0}}, 30030008, 200, 11101003, 12, 500},
	[30030008] = {30030008, '布料建筑', false, 8, false, {{169920, 0}}, 30030009, 250, 11101003, 12, 625},
	[30030009] = {30030009, '布料建筑', false, 9, false, {{365280, 0}}, 30030010, 300, 11101003, 12, 750},
	[30030010] = {30030010, '布料建筑', false, 10, false, {{785280, 0}}, false, 400, 11101003, 12, 1000},
	[30040001] = {30040001, '皮革建筑', false, 1, false, {{1000, 0}}, 30040002, 20, 11101004, 8, 50},
	[30040002] = {30040002, '皮革建筑', false, 2, false, {{2200, 0}}, 30040003, 40, 11101004, 8, 100},
	[30040003] = {30040003, '皮革建筑', false, 3, false, {{4600, 0}}, 30040004, 60, 11101004, 8, 150},
	[30040004] = {30040004, '皮革建筑', false, 4, false, {{10000, 0}}, 30040005, 80, 11101004, 8, 200},
	[30040005] = {30040005, '皮革建筑', false, 5, false, {{21400, 0}}, 30040006, 100, 11101004, 8, 250},
	[30040006] = {30040006, '皮革建筑', false, 6, false, {{46000, 0}}, 30040007, 150, 11101004, 8, 375},
	[30040007] = {30040007, '皮革建筑', false, 7, false, {{98800, 0}}, 30040008, 200, 11101004, 8, 500},
	[30040008] = {30040008, '皮革建筑', false, 8, false, {{212400, 0}}, 30040009, 250, 11101004, 8, 625},
	[30040009] = {30040009, '皮革建筑', false, 9, false, {{456600, 0}}, 30040010, 300, 11101004, 8, 750},
	[30040010] = {30040010, '皮革建筑', false, 10, false, {{981600, 0}}, false, 400, 11101004, 8, 1000},

}

local _index = {
    30010001, 30010002, 30010003, 30010004, 30010005, 30010006, 30010007, 30010008, 30010009, 30010010, 30020001, 30020002, 30020003, 30020004, 30020005, 30020006, 30020007, 30020008, 30020009, 30020010, 30030001, 30030002, 30030003, 30030004, 30030005, 30030006, 30030007, 30030008, 30030009, 30030010, 30040001, 30040002, 30040003, 30040004, 30040005, 30040006, 30040007, 30040008, 30040009, 30040010, 
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
	debug.LogErrorFormat('cf_build not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M