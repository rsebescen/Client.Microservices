const express = require('express')
var bodyParser = require('body-parser');
const app = express()

let data = [];

app.use(bodyParser.json());
app.get('/api/data', (req, res) => res.send(JSON.stringify(data)))
app.post('/api/data', (req, res) => {
    console.log(req);
    console.log(req.body);
    data.push(req.body);
    res.send(JSON.stringify(req.body));
})

app.listen(3000, () => console.log('Example app listening on port 3000!'))