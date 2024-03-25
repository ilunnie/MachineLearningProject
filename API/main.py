import numpy as np
from joblib import load
from flask import Flask, jsonify, request

decisionTree = load('Models/DecisionTree.pkl')
elasticNet = load('Models/ElasticNet.pkl')

app = Flask(__name__)

@app.route('/DecisionTree', methods=['GET'])
def DecisionTree():
    params = request.json
    if params is None or 'L' not in params:
        return jsonify({"error": "Dados ausentes ou formato inválido"}), 400
    
    L_value = np.array(params.get("L")).reshape(-1, 1)
    predict = decisionTree.predict(L_value)
    
    return jsonify(predict.tolist())

@app.route('/ElasticNet', methods=['GET'])
def ElasticNet():
    params = request.json
    if params is None or 'L' not in params:
        return jsonify({"error": "Dados ausentes ou formato inválido"}), 400
    
    L_value = np.array(params.get("L")).reshape(-1, 1)
    predict = elasticNet.predict(L_value)
    
    return jsonify(predict.tolist())
    
if __name__ == '__main__':
    app.run(debug=True)