//DOM 宣告
const title  = document.querySelector('.LOLtitle')
const url = 'https://ddragon.leagueoflegends.com/cdn/10.22.1/data/zh_TW/champion.json'
const row = document.querySelector('.LOL-cards')
let LOLArray = []
//標題文字
title.innerText = '英雄聯盟角色卡'

//window onload
window.onload = function(){
    requestJSON()
}

//AJAX

let xhr = new XMLHttpRequest()

function requestJSON() {
    xhr.onload = function(){
        LOLArray = JSON.parse(this.responseText)
        console.log(LOLArray)
        let hero = Object.values(LOLArray.data)

        hero.forEach((item, index) => {
            console.log(item)
            let card = document.querySelector('#LOLcard')
            let name = item.name
            let cloneCard = card.content.cloneNode(true)
            

            cloneCard.querySelector('h5').innerText = `${item.name} - ${item.id}`
            cloneCard.querySelector('p').innerText = item.title
            cloneCard.querySelector('img').src = `https://ddragon.leagueoflegends.com/cdn/img/champion/splash/${item.id}_0.jpg`

            cloneCard.querySelector('.btn').addEventListener('click', function(){

                let modal = document.querySelector('#exampleModal')
                modal.querySelector('h5').innerText = item.title

                let stats = ''
                let data = Object.keys(item.stats)

                data.forEach((txt) => {
                    stats = `${stats + txt} : ${item.stats[txt]} \n`
                })
                modal.querySelector('.modal-text').innerText = stats
                modal.querySelector('.modal-text-blurb').innerText = item.blurb

                modal.querySelector('#LOLImage').src = `https://ddragon.leagueoflegends.com/cdn/img/champion/splash/${item.id}_0.jpg`;
                modal.querySelector('#description').innerText = item.name +" - "+item.id;
            })
            row.append(cloneCard)
        })
    }

    xhr.open('GET', url)
    xhr.send()
}