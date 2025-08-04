let editId = null;
let allEndpoints = [];

async function loadEndpoints() {
    const res = await fetch('/config');
    allEndpoints = await res.json();
    renderEndpoints(allEndpoints);
}

function renderEndpoints(data) {
    const list = document.getElementById('endpoint-list');
    list.innerHTML = '';
    data.forEach(ep => {
        const div = document.createElement('div');
        div.className = 'endpoint-card';
        div.dataset.id = ep.Id;
        if (ep.Id === editId) div.classList.add('editing');
        div.innerHTML = `
            <p><b>${ep.Method}</b> ${ep.Route}</p>
            <pre>${ep.Response}</pre>
            <p>Status: ${ep.StatusCode}</p>
            <p>Delay: ${ep.DelayMs || 0} ms</p>
            <p><small>ID: ${ep.Id}</small></p>
            <button type="button" onclick="editEndpoint('${ep.Id}','${ep.Route}','${ep.Method}','${encodeURIComponent(ep.Response)}',${ep.StatusCode},${ep.DelayMs || 0})">Edit</button>
            <button type="button" onclick="deleteEndpoint('${ep.Id}')">Delete</button>
        `;
        list.appendChild(div);
    });
}

function editEndpoint(id, route, method, response, status, delay) {
    editId = id;
    document.getElementById('form-title').innerText = `Editing Endpoint (ID: ${id})`;
    document.getElementById('cancelEdit').style.display = 'inline-block';

    document.getElementById('route').value = route;
    document.getElementById('method').value = method;

    let json = {};
    try { json = JSON.parse(decodeURIComponent(response)); } catch { }
    document.getElementById('respKey').value = Object.keys(json)[0] || "";
    document.getElementById('respValue').value = Object.values(json)[0] || "";
    document.getElementById('statusCode').value = status;
    document.getElementById('delayMs').value = delay || 0;

    highlightEditingCard();
}

document.getElementById('cancelEdit').addEventListener('click', () => resetForm());

async function deleteEndpoint(id) {
    if (!confirm('Delete this endpoint?')) return;
    await fetch('/config/delete', {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Id: id })
    });
    if (editId === id) resetForm();
    await loadEndpoints();
    loadStats();
}

document.getElementById('endpoint-form').addEventListener('submit', async e => {
    e.preventDefault();
    const route = document.getElementById('route').value;
    const method = document.getElementById('method').value;
    const responseKey = document.getElementById('respKey').value;
    const responseValue = document.getElementById('respValue').value;
    const statusCode = parseInt(document.getElementById('statusCode').value);
    const delayMs = parseInt(document.getElementById('delayMs').value) || 0;

    if (editId) {
        const data = {
            Id: editId,
            Route: route,
            Method: method,
            Response: JSON.stringify({ [responseKey]: responseValue }),
            StatusCode: statusCode,
            DelayMs: delayMs
        };
        await fetch('/config/update', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
    } else {
        const data = {
            Route: route,
            Method: method,
            ResponseKey: responseKey,
            ResponseValue: responseValue,
            StatusCode: statusCode,
            DelayMs: delayMs
        };
        await fetch('/config/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
    }

    resetForm();
    await loadEndpoints();
    loadStats();
});

function resetForm() {
    editId = null;
    document.getElementById('endpoint-form').reset();
    document.getElementById('form-title').innerText = 'Add Endpoint';
    document.getElementById('cancelEdit').style.display = 'none';
    highlightEditingCard();
}

function highlightEditingCard() {
    document.querySelectorAll('.endpoint-card').forEach(c => c.classList.remove('editing'));
    if (editId) {
        const card = document.querySelector(`.endpoint-card[data-id='${editId}']`);
        if (card) card.classList.add('editing');
    }
}

async function loadStats() {
    const res = await fetch('/stats');
    const stats = await res.json();
    const tbody = document.querySelector('#stats-table tbody');
    tbody.innerHTML = '';
    stats.forEach(s => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${s.Route}</td><td>${s.Method}</td><td>${s.CallCount}</td>`;
        tbody.appendChild(tr);
    });
}
async function exportEndpoints() {
    const res = await fetch('/config');
    const data = await res.json();
    const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'endpoints-backup.json';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

document.getElementById('importFile').addEventListener('change', async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    const text = await file.text();
    const endpoints = JSON.parse(text);

    await fetch('/config/import', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(endpoints)
    });

    await loadEndpoints();
});


document.getElementById('refreshStats').addEventListener('click', () => loadStats());
document.getElementById('clearStats').addEventListener('click', async () => {
    if (!confirm("Clear all statistics?")) return;
    await fetch('/stats/clear', { method: 'POST' });
    loadStats();
});

document.getElementById('filterInput').addEventListener('input', e => {
    const val = e.target.value.toLowerCase();
    const filtered = allEndpoints.filter(ep =>
        ep.Route.toLowerCase().includes(val) || ep.Method.toLowerCase().includes(val)
    );
    renderEndpoints(filtered);
});

loadEndpoints();
loadStats();
